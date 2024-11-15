using Application.Exceptions;
using Domain.BaseResponse;
using Domain.DTO;
using Domain.Entities;
using Domain.IServices;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Account.Command.CommandHandler
{
    public class LoginCommandHandler(
        UserManager<ApplicationUser> _userManager,
        SignInManager<ApplicationUser> _signInManager,
        RoleManager<IdentityRole> _roleManager,
        IJwtTokenService _jwtTokenService
        ) : IRequestHandler<LoginCommand, Response<TokenResponse>>
    {
        public async Task<Response<TokenResponse?>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if (request.Login == null)
                throw new ArgumentNullException(nameof(request.Login), "Login payload cannot be null");

            // Ensure that cancellation is properly handled
            cancellationToken.ThrowIfCancellationRequested();

            // Validate that the email and password are not empty
            if (string.IsNullOrEmpty(request.Login.Email))
                throw new ArgumentException("Email cannot be empty", nameof(request.Login.Email));

            if (string.IsNullOrEmpty(request.Login.Password))
                throw new ArgumentException("Password cannot be empty", nameof(request.Login.Password));

            // Attempt to find the user by email
            ApplicationUser user = await _userManager.FindByEmailAsync(request.Login.Email);
            if (user == null)
                throw new UserNotFoundException("User with the provided email address does not exist.");

            // Check if the email is confirmed
            //if (!user.EmailConfirmed)
            //    throw new EmailNotConfirmedException("Email not verified. Please verify your email address.");

            // Attempt to sign in the user with the provided password
            SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, request.Login.Password, false, false);
            if (signInResult.Succeeded)
            {
                // If successful, generate and return the JWT token
                TokenResponse jwtToken = await _jwtTokenService.GenerateJwtToken(user, cancellationToken);
                return Response<TokenResponse?>.SuccessResponse(content: jwtToken);
            }

            // If sign-in fails, throw an appropriate exception
            throw new InvalidCredentialsException("Invalid email or password.");
        }

    }
}
