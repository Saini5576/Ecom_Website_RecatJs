using Application.DTO;
using Domain.BaseResponse;
using MediatR;

namespace Application.Features.Authentication.Command;
public record RegisterCommand(RegisterDto registerPayload) : IRequest<Response>;
