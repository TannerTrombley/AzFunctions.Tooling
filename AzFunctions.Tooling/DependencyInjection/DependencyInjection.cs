using AzFunctions.Tooling.Auth;
using AzFunctions.Tooling.Context;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzFunctions.Tooling.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection UseAllTooling(this IServiceCollection services, ValidationSettings settings)
        {
            return services.AddSingleton<ValidationSettings>(settings)
                .AddSingleton<ITokenValidator, TokenValidator>().
                AddHttpContextAccessor().
                AddScoped<ICommandContext, CommandContext>();
        }
    }
}
