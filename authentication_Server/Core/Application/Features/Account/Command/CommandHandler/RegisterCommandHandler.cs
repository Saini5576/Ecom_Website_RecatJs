using AutoMapper;
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
using Domain.DTO;
using Domain.IServices;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Application.Features.Authentication.Command.CommandHandler
{
    public class RegisterCommandHandler(
        UserManager<ApplicationUser> _userManager,
        RoleManager<IdentityRole> _roleManager,
        IEmailSender _emailSender,
        IMapper _mapper,
         ILogger<RegisterCommandHandler> _logger,
        IGetIpAddress _getIpAddress
        ) : IRequestHandler<RegisterCommand, Response>
    {

        public async Task<Response> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Check if cancellation is requested early
            cancellationToken.ThrowIfCancellationRequested();

            if (request.register == null)
            {
                return Response.FailureResponse(
                    message: "Register payload cannot be null.",
                      new ErrorModel
                    {
                        Error = "Request payload is null",
                        ErrorLocation = "RegisterCommandHandler"
                    }
                );
            }

            try
            {
                // Check if the email is already taken
                var existingUser = await _userManager.FindByNameAsync(request.register.Email);
                if (existingUser != null)
                {
                    return Response.FailureResponse(
                        message: "The email address is already in use. Please choose a different one.",
                          new ErrorModel
                        {
                            Error = "Email already in use",
                            ErrorLocation = "RegisterCommandHandler"
                        }
                    );
                }

                #region Create User
                var getIpAddress = await _getIpAddress.GetIpAddressAsync();
                if (getIpAddress == null || string.IsNullOrEmpty(getIpAddress.country) || string.IsNullOrEmpty(getIpAddress.city) || string.IsNullOrEmpty(getIpAddress.citylatlong))
                {
                    // Log the error and handle invalid IP address data
                    _logger.LogWarning("Failed to retrieve a valid IP address. Using default values.");

                    // Set default values if the IP address data is invalid
                    getIpAddress = new GetIpLocation
                    {
                        country = "Unknown",
                        city = "Unknown",
                        citylatlong = "Unknown"
                    };
                }
                // Map to ApplicationUser directly without needing a second mapping step
                var user = new ApplicationUser
                {
                    Name = request.register.Name,
                    Email = request.register.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = request.register.Email,
                    RegisteredIpAddress = $"Country: {getIpAddress.country}, City: {getIpAddress.city}, CityLatLong: {getIpAddress.citylatlong}"
                };

                // Create the user with the provided password
                var createUser = await _userManager.CreateAsync(user, request.register.Password);
                cancellationToken.ThrowIfCancellationRequested();
                #endregion Create User

                // If creation fails, return the errors as a response
                if (!createUser.Succeeded)
                {
                    var errorMessages = string.Join(", ", createUser.Errors.Select(e => e.Description));
                    _logger.LogError("User creation failed: {ErrorMessages}", errorMessages);

                    return Response.FailureResponse(
                        message: "User creation failed.",
                          new ErrorModel
                        {
                            Error = errorMessages,
                            ErrorLocation = "RegisterCommandHandler"
                        }
                    );
                }

                #region Ensure roles exist and assign the user to a role
                await EnsureRolesExist();

                var roleResult = await _userManager.AddToRoleAsync(user, RoleNames.User);
                string emailconfirmationtoken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                if (!roleResult.Succeeded)
                {
                    if (string.IsNullOrEmpty(emailconfirmationtoken))
                    {
                        return Response.FailureResponse(
                            message: "Something went wrong during role assignment.",
                              new ErrorModel
                            {
                                Error = "No email confirmation token generated.",
                                ErrorLocation = "RegisterCommandHandler"
                            }
                        );
                    }

                    // Log any role assignment failures and return a response
                    _logger.LogError("Failed to assign role to user: {RoleErrors}", string.Join(", ", roleResult.Errors.Select(e => e.Description)));

                    return Response.FailureResponse(
                        message: "User created but failed to assign roles.",
                          new ErrorModel
                        {
                            Error = string.Join(", ", roleResult.Errors.Select(e => e.Description)),
                            ErrorLocation = "RegisterCommandHandler"
                        }
                    );
                }
                #endregion Ensure roles exist and assign the user to a role

                #region ConfirmEmail
                string callback_url = "";  // You should construct a valid URL here

                string htmlMessage = string.Format(EmailBodyMessages.ConfirmEmailTemplate,
                    HtmlEncoder.Default.Encode(callback_url),
                    user.Name);

                await _emailSender.SendEmailAsync(
                    email: user.Email,
                    subject: EmailSubjects.ConfirmEmail,
                    htmlMessage: htmlMessage
                );
                #endregion ConfirmEmail

                return Response.SuccessResponse("User registered successfully.");
            }
            catch (Exception ex)
            {
                // Log unexpected exceptions
                _logger.LogError(ex, "An unexpected error occurred during user registration.");

                return Response.FailureResponse(
                    message: "An unexpected error occurred during user registration.",
                      new ErrorModel
                    {
                        Error = ex.Message,
                        ErrorLocation = "RegisterCommandHandler"
                    }
                );
            }
        }


        #region EnsureRolesExist

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
        #endregion EnsureRolesExist

    }
}
