using Domain.Entities;
using Domain.Model;

namespace Domain.IServices;
public interface IJwtTokenService
{
    ValueTask<TokenResponse> GenerateJwtToken(ApplicationUser payload, CancellationToken cancellationToken = default);
}
