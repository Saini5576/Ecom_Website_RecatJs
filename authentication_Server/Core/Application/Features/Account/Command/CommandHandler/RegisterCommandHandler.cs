﻿using AutoMapper;
using Domain.BaseResponse;
using Domain.Entities;
using Domain.Constant;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Azure.Core;
using Microsoft.AspNetCore.Identity.UI.Services;
using static Domain.Constant.EmailType;
using System.Text.Encodings.Web;

namespace Application.Features.Authentication.Command.CommandHandler
{
    public class RegisterCommandHandler(
        UserManager<ApplicationUser> _userManager,
        RoleManager<IdentityRole> _roleManager,
        IEmailSender _emailSender,
    IMapper _mapper,
         ILogger<RegisterCommandHandler> _logger
        //IEmailSender _emailSender,
        //IOptions<URLConfiguration> _config
        ) : IRequestHandler<RegisterCommand, Response>
    {

        public async Task<Response> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Check if cancellation is requested early
            cancellationToken.ThrowIfCancellationRequested();

            if (request.register == null)
            {
                throw new ArgumentNullException(nameof(request.register), "Register payload cannot be null.");
            }

            try
            {
                // Check if the email is already taken
                var existingUser = await _userManager.FindByNameAsync(request.register.Email);
                if (existingUser != null)
                {
                    return Response.FailureResponse("The email address is already in use. Please choose a different one.");
                }

                // Map to ApplicationUser directly without needing a second mapping step
                var user = new ApplicationUser
                {
                    Name = request.register.Name,
                    Email = request.register.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = request.register.Email
                };

                // Create the user with the provided password
                var createUser = await _userManager.CreateAsync(user, request.register.Password);
                cancellationToken.ThrowIfCancellationRequested();

                // If creation fails, return the errors as a response
                if (!createUser.Succeeded)
                {
                    var errorMessages = string.Join(", ", createUser.Errors.Select(e => e.Description));
                    _logger.LogError("User creation failed: {ErrorMessages}", errorMessages);
                    return Response.FailureResponse($"User creation failed: {errorMessages}");
                }

                // Ensure roles exist and assign the user to a role
                await EnsureRolesExist();

                var roleResult = await _userManager.AddToRoleAsync(user, RoleNames.User);
                string emailconfirmationtoken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                if (!roleResult.Succeeded)
                {
                    if (string.IsNullOrEmpty(emailconfirmationtoken))
                        throw new Exception("Something went wrong");

                    // Log any role assignment failures and return a response
                    _logger.LogError("Failed to assign role to user: {RoleErrors}", string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                    return Response.FailureResponse("User created but failed to assign roles.");
                }

                //string callback_url = GenerateCallbackUrl(
                //           frontendUrl: _config.Value!.FrontEndBaseURL!, email: request.registerPayload.Email,
                //           token: emailconfirmationtoken, UrlType.EmailConfirmation);

                string callback_url = "";

                string htmlMessage = string.Format(EmailBodyMessages.ConfirmEmailTemplate,
                                           HtmlEncoder.Default.Encode(callback_url),
                                           user.Name);

                await _emailSender.SendEmailAsync(
                email: user.Email,
                subject: EmailSubjects.ConfirmEmail,
                htmlMessage: htmlMessage
            );

                return Response.SuccessResponse("User registered successfully.");
            }
            catch (Exception ex)
            {
                // Log unexpected exceptions
                _logger.LogError(ex, "An unexpected error occurred during user registration.");
                return Response.FailureResponse($"An error occurred: {ex.Message}");
            }
        }

        private async Task EnsureRolesExist()
        {
            string[] roles = new[] { RoleNames.Admin, RoleNames.User };

            foreach (var roleName in roles)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    var roleCreationResult = await _roleManager.CreateAsync(new IdentityRole(roleName));

                    // Log any failures in role creation
                    if (!roleCreationResult.Succeeded)
                    {
                        _logger.LogError("Failed to create role {RoleName}: {ErrorMessages}", roleName, string.Join(", ", roleCreationResult.Errors.Select(e => e.Description)));
                    }
                }
            }
        }


    }
}
