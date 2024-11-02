using Domain.BaseResponse;
using Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Account.Command.CommandHandler
{
    public class RegisterCommandHandler(UserManager<ApplicationUser> _userManager,
        RoleManager<ApplicationRole> _roleManager) : IRequestHandler<RegisterCommand, Response>
    {
        public Task<Response> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
