using Infrastructure.context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        }
    }

}
