using Application.Common.ServiceInterfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using FluentValidation;
using Application.Common.Behaviours;
using Application.Configurations;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceConfigurations
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            JWTConfigurations jWTConfigurations = new();
            services.AddSingleton(AwsLambdaConfigurator.Bind(jWTConfigurations, configuration));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            return services;
        }
    }
}
