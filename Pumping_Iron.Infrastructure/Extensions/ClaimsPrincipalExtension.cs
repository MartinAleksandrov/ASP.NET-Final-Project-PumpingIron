﻿namespace Pumping_Iron.Infrastructure.Extensions
{
    using System.Security.Claims;
    public static class ClaimsPrincipalExtensions
    {
        public static string GetId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }
    }
}
