using AzFunctions.Tooling.Auth;
using AzFunctions.Tooling.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AzFunctions.Tooling.Context
{
    public class CommandContext : ICommandContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommandContext(IHttpContextAccessor httpContextAccessor, ITokenValidator tokenValidator)
        {
            _httpContextAccessor = httpContextAccessor;
            AuthenticationResult = tokenValidator.ValidateToken(httpContextAccessor.HttpContext.Request);
            httpContextAccessor.HttpContext.User = AuthenticationResult.Principal;
        }

        private string _displayName = null;
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_displayName))
                {
                    _displayName = _httpContextAccessor.GetCurrentDisplayName();
                }

                return _displayName;
            }
        }

        private Guid _objectId = Guid.Empty;
        public Guid ObjectId
        {
            get
            {
                if (_objectId == Guid.Empty)
                {
                    _objectId = Guid.Parse(_httpContextAccessor.GetCurrentUserId());
                }

                return _objectId;
            }
        }

        public TokenValidationResult AuthenticationResult { get; private set; }

        public bool IsAuthorized
        {
            get
            {
                // Base implementation considers all authenticated users authorized
                return AuthenticationResult.Status == TokenValidationStatus.Success;
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return AuthenticationResult.Status == TokenValidationStatus.Success;
            }
        }

        public ActionResult ExecuteAction(Func<object> action)
        {
            if (!IsAuthenticated || !IsAuthorized)
            {
                return new UnauthorizedResult();
            }

            return new OkObjectResult(action());
        }

        public async Task<ActionResult> ExecuteActionAsync(Func<Task<object>> action)
        {
            if (!IsAuthenticated || !IsAuthorized)
            {
                return await Task.FromResult(new UnauthorizedResult());
            }

            return new OkObjectResult( await action());
        }
    }
}
