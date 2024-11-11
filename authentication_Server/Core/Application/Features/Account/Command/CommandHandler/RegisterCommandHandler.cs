using Domain.BaseResponse;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Authentication.Command.CommandHandler
{
    public class RegisterCommandHandler(
        UserManager<ApplicationUser> _userManager,
        RoleManager<IdentityRole> _roleManager
        //IMapper _mapper, 
        //IEmailSender _emailSender,
        //IOptions<URLConfiguration> _config
        ) : IRequestHandler<RegisterCommand, Response>
    {
       
        public Task<Response> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
