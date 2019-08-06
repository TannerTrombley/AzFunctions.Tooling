using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace AzFunctions.Tooling.Utilities
{
    public static class HttpContextAccessorExtensions
    {
        public static string GetCurrentUserId(this IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
        }

        public static string GetCurrentTenantId(this IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid")?.Value;
        }

        public static string GetCurrentDisplayName(this IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext.User.FindFirst("name")?.Value;
        }

        public static string GetCurrentAccessToken(this IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                var headerParts = httpContextAccessor.HttpContext.Request.Headers["Authorization"][0].Split(' ');
                if (headerParts.Length >= 2)
                {
                    return headerParts[1];
                }
            }

            return null;
        }

        public static string GetCurrentUserUpn(this IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor.HttpContext?.User == null)
            {
                return null;
            }

            var user = httpContextAccessor.HttpContext.User;

            var upn = user.FindFirst(ClaimTypes.Upn)?.Value;
            if (string.IsNullOrEmpty(upn))
            {
                upn = user.FindFirst(ClaimTypes.Email)?.Value;
            }

            return upn;
        }

        public static IEnumerable<string> GetUserRoles(this IHttpContextAccessor httpContextAccessor)
        {
            var roles = httpContextAccessor.HttpContext?.User?.FindAll("wids");
            return roles != null ? roles.Select(c => c.Value) : Enumerable.Empty<string>();
        }
    }
}
