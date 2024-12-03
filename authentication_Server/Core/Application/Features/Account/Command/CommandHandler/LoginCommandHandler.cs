
using Domain.BaseResponse;
using Domain.DTO;
using Domain.Entities;
using Domain.IServices;
using Domain.Model;
using Infrastructure.Context;
using Infrastructure.ExceptionHandler.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
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
        ApplicationDbContext _context
        ) : IRequestHandler<LoginCommand, Response<LoginResponse>>
    {
        public async Task<Response<LoginResponse?>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if (request.Login == null)
            return Response<LoginResponse>.FailureResponse("Login payload cannot be null");

            // Ensure that cancellation is properly handled
            cancellationToken.ThrowIfCancellationRequested();

            ValidateLogin(request.Login);

            // Attempt to find the user by email
            ApplicationUser user = await _userManager.FindByEmailAsync(request.Login.Email);
            if (user == null)
            return Response<LoginResponse>.FailureResponse("User with the provided email address does not exist.");

            if (!user.EmailConfirmed)
            return Response<LoginResponse>.FailureResponse("Email not verified! Please verify your email.");

            // Attempt to sign in the user with the provided password
            SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, request.Login.Password, false, lockoutOnFailure: true);
            if (signInResult.Succeeded)
            {
                // If successful, generate and return the JWT token
                TokenResponse jwtToken = await _jwtTokenService.GenerateJwtToken(user, cancellationToken);

                // Generate a refresh token and update user
                await UpdateUserWithRefreshTokenAsync(user, cancellationToken);                   

                return Response<LoginResponse?>.SuccessResponse(content : new LoginResponse() { Token = jwtToken.Token,RefreshToken = user.RefreshToken,TokenExpiration = jwtToken.Expiration});
            }

            if (signInResult.IsLockedOut)
            {
                return Response<LoginResponse>.FailureResponse("Your account is temporarily locked due to multiple failed login attempts. Please try again in 2 minutes.");
            }


            // If sign-in fails, throw an appropriate exception
            return Response<LoginResponse?>.FailureResponse(
                    Message: "Invalid email or password.");
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
            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1); // Ideally, retrieve this value from configuration
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
