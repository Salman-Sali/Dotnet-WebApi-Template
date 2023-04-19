using Application.Common.Exceptions;
using Application.Common.ServiceInterfaces;
using Application.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.Services
{
    public class JWTService : IJWTService
    {
        private readonly JWTConfigurations _configurations;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public JWTService(JWTConfigurations configurations, IHttpContextAccessor httpContextAccessor)
        {
            _configurations = configurations;
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GenerateToken(List<Claim> claims)
        {
            string secretKey = _configurations.SecretKey;
            string issuer = _configurations.Issuer;
            string audience = _configurations.Audience;
            int expiryInMinutes = _configurations.ExpiryInMinutes;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expiryInMinutes),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey)),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool ValidateToken(DateTime? tokenInvalidatedDateTime)
        {
            var token = GetJwtToken();
            string secretKey = _configurations.SecretKey;
            string issuer = _configurations.Issuer;
            string audience = _configurations.Audience;
            if (tokenInvalidatedDateTime != null)
            {
                if (GetTokenIssuedDateTime() < tokenInvalidatedDateTime)
                {
                    return false;
                }
            }

            if (GetTokenExpiryDateTime() < DateTime.UtcNow)
            {
                return false;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey)),
                ValidateIssuerSigningKey = true
            };
            SecurityToken validatedToken;
            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetClaimValue(string claimKey)
        {
            var token = GetJwtToken();
            if (string.IsNullOrEmpty(token))
            {
                throw new AppException("Unauthorised.", System.Net.HttpStatusCode.Unauthorized);
            }
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = handler.ReadJwtToken(token);
            return jwtToken.Claims.Where(x => x.Type == claimKey).Select(x => x.Value).FirstOrDefault();
        }

        public DateTime GetTokenIssuedDateTime()
        {
            var tokenStr = GetJwtToken();
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenStr);
            return token.ValidFrom;
        }

        public DateTime GetTokenExpiryDateTime()
        {
            var tokenStr = GetJwtToken();
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenStr);
            return token.ValidTo;
        }

        public string? GetJwtToken()
        {
            HttpContext context = _httpContextAccessor.HttpContext;
            if (context.Request.Headers.TryGetValue("Authorization", out var token))
            {
                return token[0]?.Split(" ")[1];
            }
            return null;
        }
    }
}
