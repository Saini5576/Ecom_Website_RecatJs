using Domain.BaseResponse;
using Domain.DTO;
using Domain.IServices;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Account.Query.QueryHandler
{
    public class GetAllRolesQueryHandler(RoleManager<IdentityRole> _roleManager) : IRequestHandler<GetAllRolesQuery, Response<IEnumerable<RoleDto>>>
    {
        public async Task<Response<IEnumerable<RoleDto>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            
            // Ensure cancellation is checked early
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                // Fetch roles asynchronously with cancellation token
                var roles = await _roleManager.Roles.ToListAsync(cancellationToken);

                // Check if no roles are returned and return early
                if (roles == null || !roles.Any())
                {
                    return Response<IEnumerable<RoleDto>>.FailureResponse(
    message: "No roles found.", new ErrorModel
    {
        Error = "No roles found in the system.",
        ErrorLocation = "GetAllRolesQueryHandler"
    }
);
                }

                // Map to RoleDto using LINQ projection
                var roleDtos = roles.Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name,
                }).ToList();  // Converting to a List to improve performance on later usage
                return Response<IEnumerable<RoleDto>>.SuccessResponse(
    content: roleDtos,
    message: "Roles fetched successfully"
);

            }
            catch (OperationCanceledException)
            {
                // Handle cancellation explicitly
                return Response<IEnumerable<RoleDto>>.FailureResponse(
                    message: "Operation was canceled.", new ErrorModel
                    {
                        Error = "The operation was canceled by the user.",
                        ErrorLocation = "GetAllRolesQueryHandler"
                    }
                );
            }
            catch (Exception ex)
            {
                // Log exception (optional: depending on your logging framework)
                // _logger.LogError(ex, "An error occurred while retrieving roles.");

                return Response<IEnumerable<RoleDto>>.FailureResponse(
                    message: "An error occurred while processing your request.", new ErrorModel
                    {
                        Error = ex.Message,  // You can log more detailed exception information
                        ErrorLocation = "GetAllRolesQueryHandler"
                    }
                );
            }

        }



    }
}
