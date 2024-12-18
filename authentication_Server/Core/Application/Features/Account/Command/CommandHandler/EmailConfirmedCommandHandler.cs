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
            // Early validation for null or invalid request data
            if (request == null)
            {
                _logger.LogWarning("EmailConfirmedCommand request is null.");
                return Response.FailureResponse(
                    "Request cannot be null.",
                    new ErrorModel
                    {
                        Error = "NullRequest",
                        ErrorLocation = "EmailConfirmedCommand",
                        UserMessage = "The email confirmation request is invalid."
                    });
            }

            var emailConfirmed = request.EmailConfirmed;

            if (emailConfirmed == null)
            {
                _logger.LogWarning("EmailConfirmed data is null.");
                return Response.FailureResponse(
                    "Email confirmation data cannot be null.",
                    new ErrorModel
                    {
                        Error = "NullEmailConfirmationData",
                        ErrorLocation = "EmailConfirmedCommand",
                        UserMessage = "The email confirmation data is missing."
                    });
            }

            if (string.IsNullOrWhiteSpace(emailConfirmed.Email))
            {
                _logger.LogWarning("Email is null or empty for email confirmation request.");
                return Response.FailureResponse(
                    "Email cannot be null or empty.",
                    new ErrorModel
                    {
                        Error = "EmptyEmail",
                        ErrorLocation = "EmailConfirmedCommand",
                        UserMessage = "An email address is required to confirm your account."
                    });
            }

            if (string.IsNullOrWhiteSpace(emailConfirmed.Token))
            {
                _logger.LogWarning("Token is null or empty for email confirmation request.");
                return Response.FailureResponse(
                    "Token cannot be null or empty.",
                    new ErrorModel
                    {
                        Error = "EmptyToken",
                        ErrorLocation = "EmailConfirmedCommand",
                        UserMessage = "A valid token is required to confirm your account."
                    });
            }

            try
            { 
                _logger.LogInformation("Starting email confirmation process for user with email: {Email}.", emailConfirmed.Email);

                // Retrieve user by email
                var user = await _userManager.FindByEmailAsync(emailConfirmed.Email);
                if (user == null)
                {
                    _logger.LogWarning("User with email {Email} not found.", emailConfirmed.Email);
                    return Response.FailureResponse(
                        "User not found.",
                        new ErrorModel
                        {
                            Error = "UserNotFound",
                            ErrorLocation = "EmailConfirmedCommand",
                            UserMessage = "The specified user does not exist in the system."
                        });
                }

                // Confirm email
                var emailConfirmationResult = await _userManager.ConfirmEmailAsync(user, emailConfirmed.Token);
                if (!emailConfirmationResult.Succeeded)
                {
                    var errorMessages = string.Join(", ", emailConfirmationResult.Errors.Select(e => e.Description));
                    _logger.LogError("Email confirmation failed for user {Email}. Errors: {Errors}", emailConfirmed.Email, errorMessages);

                    return Response.FailureResponse(
                        "Email confirmation failed.",
                        new ErrorModel
                        {
                            Error = "EmailConfirmationFailed",
                            ErrorLocation = "EmailConfirmedCommand",
                            UserMessage = "The email confirmation process could not be completed. Please ensure the provided token is valid.",
                            DeveloperMessage = errorMessages
                        });
                }

                _logger.LogInformation("Email confirmed successfully for user with email: {Email}.", emailConfirmed.Email);

                return Response.SuccessResponse("Email confirmed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while confirming email for user with email: {Email}.", emailConfirmed.Email);

                return Response.FailureResponse(
                    "An unexpected error occurred.",
                    new ErrorModel
                    {
                        Error = "UnexpectedError",
                        ErrorLocation = "EmailConfirmedCommand",
                        UserMessage = "An error occurred while processing your request. Please try again later.",
                        DeveloperMessage = ex.Message
                    });
            }
        }


    }

}
