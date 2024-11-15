using Domain.BaseResponse;
using Domain.DTO;
using MediatR;

namespace Application.Features.Account.Command
{
    public record AssignRoleCommand(AssignRoleDto AssignRoleDto) : IRequest<Response>;
}
