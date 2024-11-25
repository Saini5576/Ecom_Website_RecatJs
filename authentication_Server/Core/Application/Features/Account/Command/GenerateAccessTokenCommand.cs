using Domain.BaseResponse;
using Domain.DTO;
using Domain.Model;
using MediatR;

namespace Application.Features.Account.Command
{
    public record GenerateAccessTokenCommand(RefreshTokenDto refreshToken) : IRequest<Response<TokenResponse>>;
}
