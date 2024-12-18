using Domain.BaseResponse;
using Domain.Configuration;
using Domain.DTO;
using Domain.Entities;
using Domain.IServices;
using Domain.Model;
using Infrastructure.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Account.Command.CommandHandler
{
    public class GenerateAccessTokenCommandHandler(IOptions<JwtKeyConfiguration> _options, UserManager<ApplicationUser> _userManager,IJwtTokenService _token,ApplicationDbContext _context) : IRequestHandler<GenerateAccessTokenCommand, Response<TokenResponse>>
    {
        public async Task<Response<TokenResponse>> Handle(GenerateAccessTokenCommand request, CancellationToken cancellationToken)
        {
            // Validate the request
            if (request == null)
            {
                return Response<TokenResponse>.FailureResponse(
                    "Request cannot be null",
                    new ErrorModel
                    {
                        Error = "RequestNull",
                        ErrorLocation = "GenerateAccessTokenCommandHandler",
                        UserMessage = "The request cannot be null. Please try again.",
                        DeveloperMessage = "The request object was null."
                    }
                );
            }

            // Extract the refresh token from the request
            string refreshToken = request.refreshToken?.RefreshToken;

            // Validate refresh token
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return Response<TokenResponse>.FailureResponse(
                    "Refresh token cannot be empty",
                    new ErrorModel
                    {
                        Error = "EmptyRefreshToken",
                        ErrorLocation = "GenerateAccessTokenCommandHandler",
                        UserMessage = "Refresh token is required for token generation.",
                        DeveloperMessage = "The refresh token is missing or empty."
                    }
                );
            }

            // Retrieve the user from the database using the refresh token
            ApplicationUser user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken, cancellationToken);

            // Validate the user and refresh token
            if (user == null)
            {
                // Log failed attempt for invalid user
                return Response<TokenResponse>.FailureResponse(
                    "User not found with the provided refresh token",
                    new ErrorModel
                    {
                        Error = "UserNotFound",
                        ErrorLocation = "GenerateAccessTokenCommandHandler",
                        UserMessage = "No user found with the provided refresh token.",
                        DeveloperMessage = "The user associated with the refresh token was not found."
                    }
                );
            }

            if (user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                // Consider logging invalid refresh token attempt
                return Response<TokenResponse>.FailureResponse(
                    "Invalid refresh token or token has expired",
                    new ErrorModel
                    {
                        Error = "InvalidOrExpiredToken",
                        ErrorLocation = "GenerateAccessTokenCommandHandler",
                        UserMessage = "The refresh token is either invalid or expired. Please request a new token.",
                        DeveloperMessage = "The refresh token is invalid or expired."
                    }
                );
            }

            // Generate new access token
            TokenResponse accessToken = await _token.GenerateJwtToken(user);

            // Return success response with the new access token
            return Response<TokenResponse>.SuccessResponse(
                accessToken,
                "Access token successfully generated"
            );
        }
         

        // The `GetPrincipalFromExpiredToken` function remains unchanged, but ensure you handle any potential exception.
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var secretKey = _options.Value.SecretKey;

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateLifetime = false // Allow processing expired tokens
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            try
            {
                // Validate token
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;

                // Check for valid token structure
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                return principal;
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                throw new SecurityTokenException("Token validation failed", ex);
            }
        }


    }
}
