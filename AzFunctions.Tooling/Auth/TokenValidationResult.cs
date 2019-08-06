using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace AzFunctions.Tooling.Auth
{
    public class TokenValidationResult
    {
        public ClaimsPrincipal Principal { get; private set; }

        public TokenValidationStatus Status { get; private set; }

        public Exception Exception { get; private set; }

        public static TokenValidationResult CreateSuccessResult(ClaimsPrincipal principal)
        {
            return new TokenValidationResult()
            {
                Principal = principal,
                Status = TokenValidationStatus.Success
            };
        }

        public static TokenValidationResult CreateErrorResult(Exception ex = null)
        {
            return new TokenValidationResult()
            {
                Status = TokenValidationStatus.Error,
                Exception = ex
            };
        }

        public static TokenValidationResult CreateExpiredResult()
        {
            return new TokenValidationResult()
            {
                Status = TokenValidationStatus.Expired
            };
        }

        public static TokenValidationResult CreateNoTokenResult()
        {
            return new TokenValidationResult()
            {
                Status = TokenValidationStatus.NoToken
            };
        }
    }
}
