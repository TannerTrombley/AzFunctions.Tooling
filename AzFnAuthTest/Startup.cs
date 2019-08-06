using AzFunctions.Tooling.Auth;
using AzFunctions.Tooling.Context;
using AzFunctions.Tooling.DependencyInjection;
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
            builder.Services.UseAllTooling();
        }
    }
}
