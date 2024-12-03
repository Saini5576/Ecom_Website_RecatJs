using Domain.BaseResponse;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Account.Command.CommandHandler
{
    public class EmailConfirmedCommandHandler : IRequestHandler<EmailConfirmedCommand, Response>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<EmailConfirmedCommandHandler> _logger;

        
        public EmailConfirmedCommandHandler(UserManager<ApplicationUser> userManager, ILogger<EmailConfirmedCommandHandler> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Response> Handle(EmailConfirmedCommand request, CancellationToken cancellationToken)
        {
            // Early validation for null or empty request data
            if (request == null)
            {
                _logger.LogWarning("EmailConfirmedCommand request is null.");
                return Response.FailureResponse("Request cannot be null.");
            }

            var emailConfirmed = request.EmailConfirmed;

            if (emailConfirmed == null)
            {
                _logger.LogWarning("EmailConfirmed data is null.");
                return Response.FailureResponse("Email confirmation data cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(emailConfirmed.Email))
            {
                _logger.LogWarning("Email is null or empty for email confirmation request.");
                return Response.FailureResponse("Email cannot be null or empty.");
            }

            if (string.IsNullOrWhiteSpace(emailConfirmed.Token))
            {
                _logger.LogWarning("Token is null or empty for email confirmation request.");
                return Response.FailureResponse("Token cannot be null or empty.");
            }

            try
            {
                // Log the start of email confirmation process
                _logger.LogInformation("Starting email confirmation process for user with email: {Email}.", emailConfirmed.Email);

                // Retrieve user by email
                var user = await _userManager.FindByEmailAsync(emailConfirmed.Email);

                if (user == null)
                {
                    // Log user not found scenario
                    _logger.LogWarning("User with email {Email} not found in the system.", emailConfirmed.Email);
                    return Response.FailureResponse("User not found.");
                }

                // Attempt to confirm the user's email using the provided token
                var emailConfirmationResult = await _userManager.ConfirmEmailAsync(user, emailConfirmed.Token);

                if (!emailConfirmationResult.Succeeded)
                {
                    // Log detailed errors if email confirmation fails
                    var errorMessages = string.Join(", ", emailConfirmationResult.Errors.Select(e => e.Description));
                    _logger.LogError("Email confirmation failed for user {Email}. Errors: {Errors}", emailConfirmed.Email, errorMessages);

                    return Response.FailureResponse($"Email confirmation failed. {errorMessages}");
                }

                // Log successful confirmation
                _logger.LogInformation("Email confirmed successfully for user with email: {Email}.", emailConfirmed.Email);

                return Response.SuccessResponse("Email confirmed successfully.");
            }
            catch (Exception ex)
            {
                // Log detailed exception for debugging
                _logger.LogError(ex, "An error occurred while confirming email for user with email: {Email}.", emailConfirmed.Email);

                // Return a general error response, but ensure that the exception message is logged for troubleshooting
                return Response.FailureResponse($"An error occurred while processing your request. Please try again later. Error: {ex.Message}");
            }
        }

    }

}
