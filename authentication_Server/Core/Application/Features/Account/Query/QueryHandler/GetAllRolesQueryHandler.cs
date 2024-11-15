using Domain.BaseResponse;
using Domain.DTO;
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
                    return new Response<IEnumerable<RoleDto>>(success: false, message: "No roles found.");
                }

                // Map to RoleDto using LINQ projection
                var roleDtos = roles.Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name,
                }).ToList();  // Converting to a List to improve performance on later usage

                return new Response<IEnumerable<RoleDto>>(success: true, content: roleDtos);
            }
            catch (OperationCanceledException)
            {
                // Handle cancellation explicitly
                return new Response<IEnumerable<RoleDto>>(success: false, message: "Operation was canceled.");
            }
            catch (Exception ex)
            {
                // Log exception (optional: depending on your logging framework)
                // _logger.LogError(ex, "An error occurred while retrieving roles.");

                return new Response<IEnumerable<RoleDto>>(success: false, message: "An error occurred while processing your request.");
            }
        }



    }
}
