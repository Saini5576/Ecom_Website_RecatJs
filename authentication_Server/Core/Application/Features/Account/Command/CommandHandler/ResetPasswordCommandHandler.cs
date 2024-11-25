using Domain.BaseResponse;
using Domain.DTO;
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
    public class ResetPasswordCommandHandler(UserManager<ApplicationUser> _userManager, ILogger<ResetPasswordCommandHandler> _logger) : IRequestHandler<ResetPasswordCommand, Response>
    {
        public async Task<Response> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            // Check if cancellation is requested at the start
            cancellationToken.ThrowIfCancellationRequested();
                
            // Validate input parameters
            ValidateResetPasswordRequest(request.resetPassword);

            // Find user by email
            var findUser = await _userManager.FindByEmailAsync(request.resetPassword.Email);
            if (findUser is null)
            {
                // Log the error for traceability and throw a more specific exception
                _logger.LogWarning($"Password reset attempt failed for non-existing user: {request.resetPassword.Email}");
                throw new  ArgumentNullException($"User with email {request.resetPassword.Email} not found. Please enter a valid email.");
            }

            // Check for cancellation again before proceeding with the password reset
            cancellationToken.ThrowIfCancellationRequested();

            // Process password reset
            var result = await ResetUserPassword(findUser, request.resetPassword.Token, request.resetPassword.NewPassword);

            // Return success or throw a more detailed exception based on the result
            return result.Succeeded
                ? Response.SuccessResponse("Successfully reset the password.")
                : HandleResetPasswordErrors(result.Errors);
        }

        private void ValidateResetPasswordRequest(ResetPasswordDto? resetPassword)
        {
            if (resetPassword == null)
                throw new ArgumentNullException(nameof(ResetPasswordDto), "ResetPasswordDto cannot be null.");

            if (string.IsNullOrWhiteSpace(resetPassword.Email))
                throw new ArgumentException("Email cannot be empty.", nameof(resetPassword.Email));

            if (string.IsNullOrWhiteSpace(resetPassword.NewPassword))
                throw new ArgumentException("New password cannot be empty.", nameof(resetPassword.NewPassword));

            if (string.IsNullOrWhiteSpace(resetPassword.Token))
                throw new ArgumentException("Password reset token cannot be empty.", nameof(resetPassword.Token));
        }

        private async Task<IdentityResult> ResetUserPassword(ApplicationUser user, string token, string newPassword)
        {
            token = token.Replace(" ", "+"); // Ensure token formatting is correct
            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }

        private Response HandleResetPasswordErrors(IEnumerable<IdentityError> errors)
        {
            // Log the errors for traceability
            var errorMessages = errors.Select(e => e.Description).ToList();
            _logger.LogError("Password reset failed: " + string.Join(", ", errorMessages));

            // Aggregate and throw a custom exception
            throw new Exception($"Password reset failed: {string.Join(", ", errorMessages)}");
        }
    }
}
