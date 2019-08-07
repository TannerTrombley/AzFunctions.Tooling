using AzFunctions.Tooling.Auth;
using AzFunctions.Tooling.DependencyInjection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(AzFnAuthTest.Startup))]
namespace AzFnAuthTest
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var authenticationSettings = new ValidationSettings()
            {
                TenantDomain = "woodsplit.onmicrosoft.com",
                TenantId = "9f08c85b-4115-499d-b09d-4fe05f69c9cb",
                ValidAudiences = new string[] { "1fec7da5-70a7-4740-b920-67354eb810ef" },
                StsHost = "https://woodsplit.b2clogin.com",
                Policy = "B2C_1_SiUpIn"
            };
            builder.Services.UseAllTooling(authenticationSettings);
        }
    }
}
