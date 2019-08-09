using AzFunctions.Tooling.Auth;
using AzFunctions.Tooling.Context;
using AzFunctions.Tooling.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace AzFunctions.Tooling.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection UseAllTooling(this IServiceCollection services)
        {
            return services
                .AddSingleton<ITokenValidator, TokenValidator>().
                AddHttpContextAccessor().
                AddScoped<ICommandContext, CommandContext>();
        }

        public static IServiceCollection UseSettingsProvider(this IServiceCollection services)
        {
            return services.AddSingleton<ISettingsProvider, SettingsProvider>();
        }
    }
}
