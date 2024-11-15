using Domain.BaseResponse;
using Domain.Configuration;
using Domain.Entities;
using Domain.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services;
public class JwtTokenService(
       UserManager<ApplicationUser> _userManager,
       RoleManager<IdentityRole> _roleManager,
       IOptions<JwtKeyConfiguration> _options,
       IConfiguration _configuration
    ) : IJwtTokenService
{
    public async ValueTask<TokenResponse> GenerateJwtToken(ApplicationUser payload, CancellationToken cancellationToken)
    {
        // Check if cancellation was requested
        cancellationToken.ThrowIfCancellationRequested();

        // Ensure the payload is not null
        if (payload is null)
        {
            throw new ArgumentNullException(nameof(payload), "Payload cannot be null or empty.");
        }

        // Ensure that SecretKey and TokenExpireDay are configured
        var secretKey = _options.Value.SecretKey;
        if (string.IsNullOrEmpty(secretKey))
        {
            throw new Exception("Token SecretKey cannot be empty or null.");
        }

        // Set the token expiration from configuration
        var expiryMinutes = _options.Value.ExpiryMinutes;

        // Create a new JWT token handler
        var tokenHandler = new JwtSecurityTokenHandler();

        // Define the token descriptor with claims and signing credentials
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(await GetClaims(payload, cancellationToken) ?? Enumerable.Empty<Claim>()),
            Expires = DateTime.Now.AddMinutes(expiryMinutes),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), // Use UTF8 encoding for better security
                SecurityAlgorithms.HmacSha256Signature) // HMAC SHA-256 algorithm for signing
        };

        // Create the JWT token
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(securityToken);

        // If token is null, throw an exception
        if (string.IsNullOrEmpty(token))
        {
            throw new Exception("Something went wrong. JWT token could not be generated.");
        }

        // Return the token response with expiration date
        return new TokenResponse(token, DateTime.UtcNow.AddMinutes(expiryMinutes));
    }

    private async ValueTask<List<Claim>> GetClaims(ApplicationUser claim, CancellationToken cancellationToken = default)
    {
        if (claim is null)
            throw new ArgumentNullException(nameof(claim));
        List<Claim> claims = new List<Claim>
        {
            new("UserId",claim.Id.ToString()),
            new("User",claim.Email),
        };
        cancellationToken.ThrowIfCancellationRequested();
        IList<string> claimRole = await _userManager.GetRolesAsync(claim);
        if (claimRole.Count > 0)
        {
            foreach (string role in claimRole ?? Enumerable.Empty<string>())
            {
                IdentityRole? findRole = await _roleManager.FindByNameAsync(role);
                if (!string.IsNullOrWhiteSpace(findRole?.Name))
                {
                    claims.Add(new Claim(ClaimTypes.Role, findRole.Name.ToString()));
                }
            }
        }
        return claims;

    }

}
