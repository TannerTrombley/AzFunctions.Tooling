using AzFunctions.Tooling.Auth;
using AzFunctions.Tooling.Context;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: FunctionsStartup(typeof(AzFnAuthTest.Startup))]
namespace AzFnAuthTest
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<ITokenValidator, TokenValidator>();
            builder.Services.AddLogging();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ICommandContext, CommandContext>();
        }
    }
}
