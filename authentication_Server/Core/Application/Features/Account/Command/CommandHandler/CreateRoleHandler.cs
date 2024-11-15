using Domain.BaseResponse;
using Domain.DTO;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Account.Command.CommandHandler
{
    public class CreateRoleHandler(RoleManager<IdentityRole> _roleManager) : IRequestHandler<CreateRoleCommand, Response>
    {

        public async Task<Response> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (await _roleManager.RoleExistsAsync(request.role))
            {
                return new Response { Success = false, Message = "Role already exists." };
            }

            var identityRole = new IdentityRole(request.role);

            cancellationToken.ThrowIfCancellationRequested();
            var result = await _roleManager.CreateAsync(identityRole);
            if (!result.Succeeded)
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                return new Response { Success = false, Message = $"Role creation failed: {errorMessage}" };
            }
            AssignRoleDto role = new AssignRoleDto();

          return new Response { Success = true, Message = "Role created successfully." };
        }

    }
}
