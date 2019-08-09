using AzFunctions.Tooling.Auth;
using AzFunctions.Tooling.DependencyInjection;
using AzFunctions.Tooling.Settings;
using AzFunctions.Tooling.Storage;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(AzFnAuthTest.Startup))]
namespace AzFnAuthTest
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.UseSettingsProvider();
            builder.Services.UseAllTooling();
            builder.Services.AddSingleton<IRepository<User>>((provider) => {
                var settings = provider.GetRequiredService<ISettingsProvider>();
                return new Repository<User>(settings.GetRequiredSetting("StorageConnectionString"), settings.GetRequiredSetting("UsersTableName"));
            });
        }
    }
}
