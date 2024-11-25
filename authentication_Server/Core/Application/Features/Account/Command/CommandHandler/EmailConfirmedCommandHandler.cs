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
            if (request == null)
                throw new ArgumentNullException(nameof(request), "Request cannot be null.");

            var emailConfirmed = request.EmailConfirmed;
            if (emailConfirmed == null)
                throw new ArgumentNullException(nameof(request.EmailConfirmed), "Email confirmation data cannot be null.");

            if (string.IsNullOrWhiteSpace(emailConfirmed.Email))
                throw new ArgumentException("Email cannot be null or empty.", nameof(emailConfirmed.Email));

            if (string.IsNullOrWhiteSpace(emailConfirmed.Token))
                throw new ArgumentException("Token cannot be null or empty.", nameof(emailConfirmed.Token));

            try
            {
                // Log the start of email confirmation process
                _logger.LogInformation("Attempting to confirm email for user with email: {Email}.", emailConfirmed.Email);

                // Find the user by email
                var findUser = await _userManager.FindByEmailAsync(emailConfirmed.Email);
                if (findUser == null)
                {
                    // Log if user is not found
                    _logger.LogWarning("User with email {Email} not found.", emailConfirmed.Email);
                    return new Response
                    {
                        Success = false,
                        Message = "User not found."
                    };
                }

                // Confirm the user's email
                var emailConfirmationResult = await _userManager.ConfirmEmailAsync(findUser, emailConfirmed.Token);
                if (!emailConfirmationResult.Succeeded)
                {
                    // Log error if email confirmation fails
                    _logger.LogError("Email confirmation failed for user {Email}. Errors: {Errors}",
                        emailConfirmed.Email, string.Join(", ", emailConfirmationResult.Errors.Select(e => e.Description)));

                    return new Response
                    {
                        Success = false,
                        Message = "Email confirmation failed."
                    };
                }

                // Log successful confirmation
                _logger.LogInformation("Email confirmed successfully for user {Email}.", emailConfirmed.Email);

                return new Response
                {
                    Success = true,
                    Message = "Email confirmed successfully."
                };
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while confirming email for {Email}.", emailConfirmed.Email);

                return new Response
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                };
            }
        }
    }

}
