using Domain.BaseResponse;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Account.Command.CommandHandler
{
    public class AssignRoleCommandHandler(UserManager<ApplicationUser> _userManager,ILogger<AssignRoleCommand> _logger) : IRequestHandler<AssignRoleCommand, Response>
    {
            public async Task<Response> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
            {
                // Input validation
                if (request?.AssignRoleDto == null)
                {
                    return new Response
                    {
                        Success = false,
                        Message = "Invalid request data. AssignRoleDto cannot be null."
                    };
                }

                if (string.IsNullOrEmpty(request.AssignRoleDto.Email))
                {
                    return new Response
                    {
                        Success = false,
                        Message = "Email is required."
                    };
                }

                if (string.IsNullOrEmpty(request.AssignRoleDto.Role))
                {
                    return new Response
                    {
                        Success = false,
                        Message = "Role is required."
                    };
                }

                try
                {
                    // Find user by email
                    var user = await _userManager.FindByNameAsync(request.AssignRoleDto.Email);
                    if (user == null)
                    {
                        return new Response
                        {
                            Success = false,
                            Message = $"User with email {request.AssignRoleDto.Email} not found."
                        };
                    }

                    // Add user to role
                    var result = await _userManager.AddToRoleAsync(user, request.AssignRoleDto.Role);

                    if (!result.Succeeded)
                    {
                        // Collect errors from the result and return a detailed response
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        return new Response
                        {
                            Success = false,
                            Message = $"Failed to assign role: {errors}"
                        };
                    }

                    return new Response
                    {
                        Success = true,
                        Message = $"User {user.UserName} successfully assigned to role {request.AssignRoleDto.Role}."
                    };
                }
                catch (Exception ex)
                {
                    // Log the exception
                    _logger.LogError(ex, "Error assigning role to user");

                    return new Response
                    {
                        Success = false,
                        Message = "An unexpected error occurred while assigning the role."
                    };
                }
            }
    }
}
