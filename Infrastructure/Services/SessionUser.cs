using Application.Common.Exceptions;
using Application.Common.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Services
{
    public class SessionUser : ISessionUser
    {
        private readonly IJWTService jwtService;

        public SessionUser(IJWTService jwtService)
        {
            this.jwtService = jwtService;
        }

        public Guid Id 
        {
            get
            {
                var userId = jwtService.GetClaimValue("userId");
                if(Guid.TryParse(userId, out var result))
                {
                    return result;
                }
                throw new AppException("No user logon.", System.Net.HttpStatusCode.InternalServerError);
            }

        }

    }
}
