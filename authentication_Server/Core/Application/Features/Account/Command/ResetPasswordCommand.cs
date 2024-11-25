using Domain.BaseResponse;
using Domain.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Account.Command
{
    public record ResetPasswordCommand(ResetPasswordDto resetPassword) : IRequest<Response>;
}
