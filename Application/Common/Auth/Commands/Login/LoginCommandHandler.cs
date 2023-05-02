using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Application.Common.Exceptions;
using Application.Common.ServiceInterfaces;
using System.Security.Claims;

namespace Application.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IMainDbContext _context;
        private readonly IJWTService jwtService;

        public LoginCommandHandler(IMainDbContext context, IJWTService jwtService)
        {
            _context = context;
            this.jwtService = jwtService;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.Where(a => a.Name == request.Name).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new AppException("Unauthorised", System.Net.HttpStatusCode.Unauthorized);
            }
            var result = (new PasswordHasher<object?>()).VerifyHashedPassword(null, user.Password, request.Password);
            if(result == PasswordVerificationResult.Failed)
            {
                throw new AppException("Unauthorised", System.Net.HttpStatusCode.Unauthorized);
            }
            var claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString())
            };
            return jwtService.GenerateToken(claims);
        }
    }
}
