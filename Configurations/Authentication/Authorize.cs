using Application.Common.Exceptions;
using Application.Common.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Configurations.Authentication
{
    public class Authorize : IAsyncActionFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJWTService jwtService;

        public Authorize(IHttpContextAccessor httpContextAccessor, IJWTService jwtService)
        {
            _httpContextAccessor = httpContextAccessor;
            this.jwtService = jwtService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!jwtService.ValidateToken())
            {
                throw new AppException("Unauthorised.", System.Net.HttpStatusCode.Unauthorized);
            }
            await next();
        }
    }
}
