using FluentEmail.Core;
using FluentEmail.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EmailMessageSender(IFluentEmail _fluentEmail) : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            if (IsValid(email, subject, htmlMessage))
            {
                await ExecuteMailAsync(email, subject, htmlMessage);
            }
            await Task.FromResult(false);
        }
        private async ValueTask<bool> ExecuteMailAsync(string email, string subject, string body)
        {
            try
            {
                // Attempt to send the email
                SendResponse response = await _fluentEmail
                                              .To(email)
                                              .Subject(subject)
                                              .Body(body, isHtml: true)
                                              .SendAsync();

                // Return success status
                return response.Successful;
            }
            catch (SmtpException smtpEx)
            {
                // Log detailed SMTP error including the SMTP response
                Console.WriteLine($"SMTP Error: {smtpEx.Message}");
                if (smtpEx.InnerException != null)
                {
                    Console.WriteLine($"SMTP Inner Exception: {smtpEx.InnerException.Message}");
                }

                // Additional logging for SMTP-specific errors
                Console.WriteLine($"Failed to send email to: {email}");
                Console.WriteLine($"Subject: {subject}");

                // More specific action can be taken based on SMTP error codes (e.g., 5.7.0 Authentication Required)
                if (smtpEx.Message.Contains("Authentication Required"))
                {
                    Console.WriteLine("Authentication failed. Please check your SMTP credentials or configuration.");
                }

                throw; // Rethrow the exception to allow higher-level handling
            }
            catch (Exception ex)
            {
                // Log general errors that are not SMTP-related
                Console.WriteLine($"General Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }

                // Additional context for general errors
                Console.WriteLine($"Error occurred while sending email to: {email}");
                Console.WriteLine($"Subject: {subject}");

                throw; // Rethrow the exception to allow higher-level handling
            }
        }

        private bool IsValid(string email, string subject, string body)
        {
            return string.IsNullOrEmpty(email) || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(body) ? false : true;
        }
    }
}
