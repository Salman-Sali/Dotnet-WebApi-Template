using Application.Common.ServiceInterfaces;
using Infrastructure.Context;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public static class ServiceConfigurations
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MainDbContext>((sp, options) =>
            {
                options.UseNpgsql(configuration.GetValue<string>("ConnectionString"), builder => builder.MigrationsAssembly(typeof(MainDbContext).Assembly.FullName));
            });

            services.AddScoped<IJWTService, JWTService>();
            return services;
        }
    }
}
