using Domain.BaseResponse;
using Domain.DTO;
using MediatR;

namespace Application.Features.Authentication.Command;
public record RegisterCommand(RegisterDto register) : IRequest<Response>;
