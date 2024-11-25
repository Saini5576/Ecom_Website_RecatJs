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
                throw new ArgumentNullException(nameof(request), "Request cannot be null");
            }

            // Extract tokens from the request
            string refreshToken = request.refreshToken.RefreshToken;
            

            // Retrieve the user from the database
            ApplicationUser user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken, cancellationToken);

            // Validate the user and refresh token
            if (user == null)
            {
                // Log failed attempt for invalid user
                return Response<TokenResponse>.FailureResponse("User not found");
            }

            if (user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                // Consider logging invalid refresh token attempt
                return Response<TokenResponse>.FailureResponse("Invalid refresh token or token has expired");
            }

            // Generate new access token
            TokenResponse accessToken = await _token.GenerateJwtToken(user);            

            // Return success response with the new access token
            return new Response<TokenResponse>(content: accessToken, message: "Access token successfully generated", success: true);
        }


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
