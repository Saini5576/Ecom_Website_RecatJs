
using Domain.BaseResponse;
using Domain.DTO;
using Domain.Entities;
using Domain.IServices;
using Domain.Model;
using Infrastructure.Context;
using Infrastructure.ExceptionHandler.Exceptions;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Account.Command.CommandHandler
{
    public class LoginCommandHandler(
        UserManager<ApplicationUser> _userManager,
        SignInManager<ApplicationUser> _signInManager,
        RoleManager<IdentityRole> _roleManager,
        IJwtTokenService _jwtTokenService,
        ApplicationDbContext _context,
        IGetIpAddress _getIpAddress,
        ILogger<LoginCommandHandler> _logger
        ) : IRequestHandler<LoginCommand, Response<LoginResponse>>
    {
        public async Task<Response<LoginResponse?>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if (request.Login == null)
            {
                return Response<LoginResponse>.FailureResponse(
                    message: "Login payload cannot be null.",
                      new ErrorModel
                    {
                        Error = "Login payload cannot be null",
                        ErrorLocation = "LoginCommandHandler"
                    }
                );
            }

            // Ensure cancellation is properly handled
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                // Validate login input
                ValidateLogin(request.Login);

                // Attempt to find the user by email
                ApplicationUser user = await _userManager.FindByEmailAsync(request.Login.Email);
                if (user == null)
                {
                    return Response<LoginResponse>.FailureResponse(
                        message: "No user found with the provided email address.",
                          new ErrorModel
                        {
                            Error = "User with the provided email address does not exist.",
                            ErrorLocation = "LoginCommandHandler"
                        }
                    );
                }

                // Uncomment and handle email confirmation if necessary
                //if (!user.EmailConfirmed)
                //{
                //    return Response<LoginResponse>.FailureResponse("Email not verified! Please verify your email.");
                //}

                // Attempt to sign in the user with the provided password
                SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, request.Login.Password, false, lockoutOnFailure: true);
                if (signInResult.Succeeded)
                {
                    // If successful, generate and return the JWT token
                    TokenResponse jwtToken = await _jwtTokenService.GenerateJwtToken(user, cancellationToken);

                    // Generate a refresh token and update the user details
                    await UpdateUserWithRefreshTokenAsync(user, cancellationToken);

                    // Success response with LoginResponse content
                    var loginResponse = new LoginResponse
                    {
                        Token = jwtToken.Token,
                        RefreshToken = user.RefreshToken,
                        TokenExpiration = jwtToken.Expiration
                    };

                    return Response<LoginResponse?>.SuccessResponse(content: loginResponse);
                }

                // Handle account lockout scenario
                if (signInResult.IsLockedOut)
                {
                    return Response<LoginResponse>.FailureResponse(
                        message: "Your account is temporarily locked due to multiple failed login attempts. Please try again later.",
                          new ErrorModel
                        {
                            Error = "Account temporarily locked due to failed login attempts.",
                            ErrorLocation = "LoginCommandHandler"
                        }
                    );
                }

                // If sign-in fails, return invalid email or password error
                return Response<LoginResponse?>.FailureResponse(
                    message: "Invalid email or password.",
                      new ErrorModel
                    {
                        Error = "Invalid email or password.",
                        ErrorLocation = "LoginCommandHandler"
                    }
                );
            }
            catch (ArgumentException ex)
            {
                return Response<LoginResponse>.FailureResponse(
                    message: $"Invalid input: {ex.Message}",
                      new ErrorModel
                    {
                        Error = ex.Message,
                        ErrorLocation = "LoginCommandHandler"
                    }
                );
            }
            catch (Exception ex)
            {
                // Log unexpected errors for debugging
                _logger.LogError(ex, "An unexpected error occurred during the login process.");
                return Response<LoginResponse>.FailureResponse(
                    message: "An unexpected error occurred. Please try again later.",
                      new ErrorModel
                    {
                        Error = "An unexpected error occurred.",
                        ErrorLocation = "LoginCommandHandler"
                    }
                );
            }
        }

        #region ValidateLogin
        private void ValidateLogin(LoginDto login)
        {
            if (string.IsNullOrEmpty(login.Email))
                throw new ArgumentException("Email cannot be empty", nameof(login.Email));

            if (string.IsNullOrEmpty(login.Password))
                throw new ArgumentException("Password cannot be empty", nameof(login.Password));
        }
        #endregion ValidateLogin

        #region UpdateUserWithRefreshTokenAsync
        private async Task UpdateUserWithRefreshTokenAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            // Get the IP address
            var getIpAddress = await _getIpAddress.GetIpAddressAsync();
            if (getIpAddress == null || string.IsNullOrEmpty(getIpAddress.country) || string.IsNullOrEmpty(getIpAddress.city) || string.IsNullOrEmpty(getIpAddress.citylatlong))
            {
                // Log the error and handle invalid IP address data
                _logger.LogWarning("Failed to retrieve a valid IP address. Using default values.");

                // Set default values if the IP address data is invalid
                getIpAddress = new GetIpLocation
                {
                    country = "Unknown",
                    city = "Unknown",
                    citylatlong = "Unknown"
                };
            }
            user.CurrentLoginIpAddress = $"Country : {getIpAddress.country}, City : {getIpAddress.city}, CityLatLong : {getIpAddress.citylatlong}";
            user.LastLoginTime = DateTime.Now;

            // Generate new refresh token and set expiry time
            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1); // Retrieve this value from configuration if needed

            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken); // Use async save method
        }
        #endregion UpdateUserWithRefreshTokenAsync

        #region GenerateRefreshToken
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        #endregion GenerateRefreshToken

    }
}
