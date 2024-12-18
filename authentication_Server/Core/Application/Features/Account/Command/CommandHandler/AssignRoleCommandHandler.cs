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
                return Response.FailureResponse(
                    "Invalid request data. AssignRoleDto cannot be null.",
                    new ErrorModel
                    {
                        ErrorLocation = "AssignRoleCommandHandler",
                        UserMessage = "Invalid request data provided."
                    });
            }

            if (string.IsNullOrEmpty(request.AssignRoleDto.Email))
            {
                return Response.FailureResponse(
                    "Email is required.",
                    new ErrorModel
                    {
                        ErrorLocation = "AssignRoleCommandHandler",
                        UserMessage = "The email field is mandatory for assigning roles."
                    });
            }

            if (string.IsNullOrEmpty(request.AssignRoleDto.Role))
            {
                return Response.FailureResponse(
                    "Role is required.",
                    new ErrorModel
                    {
                        ErrorLocation = "AssignRoleCommandHandler",
                        UserMessage = "The role field is mandatory for assigning roles."
                    });
            }

            try
            {
                // Find user by email
                var user = await _userManager.FindByNameAsync(request.AssignRoleDto.Email);
                if (user == null)
                {
                    return Response.FailureResponse(
                        $"User with email {request.AssignRoleDto.Email} not found.",
                        new ErrorModel
                        {
                            ErrorLocation = "AssignRoleCommandHandler",
                            UserMessage = "The specified user could not be located."
                        });
                }

                // Add user to role
                var result = await _userManager.AddToRoleAsync(user, request.AssignRoleDto.Role);

                if (!result.Succeeded)
                {
                    // Collect errors from the result and return a detailed response
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return Response.FailureResponse(
                        $"Failed to assign role: {errors}",
                        new ErrorModel
                        {
                            ErrorLocation = "AssignRoleCommandHandler",
                            UserMessage = "There was an issue assigning the specified role.",
                            DeveloperMessage = errors
                        });
                }

                return Response.SuccessResponse($"User {user.UserName} successfully assigned to role {request.AssignRoleDto.Role}.");
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error assigning role to user");

                return Response.FailureResponse(
                    "An unexpected error occurred while assigning the role.",
                    new ErrorModel
                    {
                        ErrorLocation = "AssignRoleCommandHandler",
                        UserMessage = "An error occurred while processing your request.",
                        DeveloperMessage = ex.Message
                    });
            }
        }

    }
}
