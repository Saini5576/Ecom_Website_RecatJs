

using Application.DTO;
using Domain.BaseResponse;
using MediatR;

namespace Application.Features.Account.Command
{
    public record RegisterCommand(RegisterDto registerPayload) : IRequest<Response>;
}
