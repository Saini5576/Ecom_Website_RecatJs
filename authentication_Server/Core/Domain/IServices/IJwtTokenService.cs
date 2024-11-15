

using Domain.BaseResponse;
using Domain.Entities;

namespace Domain.IServices;
public interface IJwtTokenService
{
    ValueTask<TokenResponse> GenerateJwtToken(ApplicationUser payload, CancellationToken cancellationToken = default);
}
