﻿using System.Security.Claims;

namespace Application.Common.ServiceInterfaces
{
    public interface IJWTService
    {
        public string? GenerateToken(List<Claim> claims);
    }
}