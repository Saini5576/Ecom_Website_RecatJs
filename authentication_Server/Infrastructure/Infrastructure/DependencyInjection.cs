using Domain.Configuration;
using Domain.IServices;
using Infrastructure.Context;
using Infrastructure.ExceptionHandler;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;

using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
  options.UseSqlServer(
      configuration.GetConnectionString("BaseDBConnection"),
      b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
         services.AddScoped<IJwtTokenService, JwtTokenService>();
         services.AddScoped<IEmailSender, EmailMessageSender>();
         services.Configure<URLConfiguration>(configuration.GetSection(nameof(URLConfiguration)).Bind);
         SmtpSettings? emailSettings = configuration.GetSection(nameof(SmtpSettings)).Get<SmtpSettings>();
         services.AddFluentEmail(
            emailSettings!.DefaultFromEmail
            ?? throw new Exception($"{emailSettings.DefaultFromEmail},Could not be null or empty"))
            .AddSmtpSender(new SmtpClient(
                emailSettings.PrimaryDomain
                ?? throw new Exception($"{emailSettings.PrimaryDomain},Could not be null or empty"))
            {
                Port = emailSettings.PrimaryPort,
                EnableSsl = true,
                Credentials = new NetworkCredential(
                    emailSettings.Username ?? string.Empty,
                    emailSettings.Password ?? string.Empty),
            });

        }     
        }
        
}
