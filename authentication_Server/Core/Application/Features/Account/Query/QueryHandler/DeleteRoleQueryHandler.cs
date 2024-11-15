using Domain.BaseResponse;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Account.Query.QueryHandler
{
    public class DeleteRoleQueryHandler(RoleManager<IdentityRole> _roleManager) : IRequestHandler<DeleteRoleQuery, Response>
    {
        public async Task<Response> Handle(DeleteRoleQuery request, CancellationToken cancellationToken)
        {
            // Early return if cancellation is requested
            cancellationToken.ThrowIfCancellationRequested();

            // Attempt to find the role by ID
            IdentityRole role = await _roleManager.FindByIdAsync(request.deleteRoleId);

            // Check if the role was found
            if (role == null)
            {
                return new Response(success: false, message: "Role not found.");
            }

            try
            {
                // Perform the deletion
                IdentityResult deleteResult = await _roleManager.DeleteAsync(role);

                // Check if the deletion was successful
                if (deleteResult.Succeeded)
                {
                    return new Response(success: true, message: "Role deleted successfully.");
                }
                else
                {
                    // Return all errors from the IdentityResult
                    var errorMessages = string.Join(", ", deleteResult.Errors.Select(e => e.Description));
                    return new Response(success: false, message: $"Failed to delete role: {errorMessages}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception if necessary (optional)
                // _logger.LogError(ex, "An error occurred while deleting the role.");

                return new Response(success: false, message: "An unexpected error occurred while attempting to delete the role.");
            }
        }

    }
}
