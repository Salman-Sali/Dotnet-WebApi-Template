using Application.Common.ServiceInterfaces;
using Infrastructure.Context;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Context.Interceptors;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceConfigurations
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IJWTService, JWTService>();
            services.AddScoped<ISessionUser, SessionUser>();
            services.AddScoped<AuditableEntitySaveChangesInterceptor>();
            services.AddDbContext<MainDbContext>((sp, options) =>
            {
                options.UseNpgsql(configuration.GetValue<string>("ConnectionString"), builder => builder.MigrationsAssembly(typeof(MainDbContext).Assembly.FullName));
            });

            return services;
        }
    }
}
