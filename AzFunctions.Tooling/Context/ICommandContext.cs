using AzFunctions.Tooling.Auth;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AzFunctions.Tooling.Context
{
    public interface ICommandContext
    {
        Guid ObjectId { get; }

        TokenValidationResult AuthenticationResult { get; }

        bool IsAuthorized { get; }

        bool IsAuthenticated { get; }

        ActionResult ExecuteAction(Func<object> action);

        Task<ActionResult> ExecuteActionAsync(Func<Task<object>> action);

    }
}
