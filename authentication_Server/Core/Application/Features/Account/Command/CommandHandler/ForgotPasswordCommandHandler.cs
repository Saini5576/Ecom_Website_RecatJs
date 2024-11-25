﻿using Domain.BaseResponse;
using Domain.Configuration;
using Domain.Constant;
using Domain.DTO;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using static Domain.Constant.EmailType;

namespace Application.Features.Account.Command.CommandHandler
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Response>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOptions<URLConfiguration> _options;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ForgotPasswordCommandHandler> _logger;

        public ForgotPasswordCommandHandler(
            UserManager<ApplicationUser> userManager,
            IOptions<URLConfiguration> options,
            IEmailSender emailSender,
            ILogger<ForgotPasswordCommandHandler> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Response> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Guard clause to handle empty email
            if (string.IsNullOrWhiteSpace(request.ForgotPassword?.Email))
            {
                _logger.LogWarning("ForgotPassword request failed: Email is required.");
                throw new ArgumentNullException(nameof(request.ForgotPassword.Email), "Email address cannot be empty.");
            }

            ApplicationUser user = await _userManager.FindByEmailAsync(request.ForgotPassword.Email);
            if (user == null)
            {
                _logger.LogInformation("ForgotPassword request: User not found for email: {Email}", request.ForgotPassword.Email);
                return Response.FailureResponse("No user found with the provided email address.");
            }

            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (string.IsNullOrEmpty(resetToken))
            {
                _logger.LogError("Failed to generate password reset token for user: {UserId}", user.Id);
                return Response.FailureResponse("Unable to generate password reset token at this time.");
            }

            string callbackUrl = EmailType.GenerateCallbackUrl(
                frontendUrl: _options.Value.FrontEndBaseURL!,
                email: request.ForgotPassword.Email,
                token: resetToken,
                UrlType.ForgetPassword
            );

            string htmlMessage = string.Format(EmailBodyMessages.ForgotPasswordTemplate,
                                               HtmlEncoder.Default.Encode(callbackUrl),
                                               user.UserName);

            try
            {
                await _emailSender.SendEmailAsync(
                    email: request.ForgotPassword.Email,
                    subject: EmailSubjects.ForgotPassword,
                    htmlMessage: htmlMessage
                );

                _logger.LogInformation("ForgotPassword email sent to {Email}", request.ForgotPassword.Email);
                return Response.SuccessResponse("Password reset email sent.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending password reset email to {Email}", request.ForgotPassword.Email);
                return Response.FailureResponse("An error occurred while sending the password reset email.");
            }
        }
    }

}

