using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using AzFunctions.Tooling.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace AzFunctions.Tooling.Auth
{
    public class TokenValidator : ITokenValidator
    {
        private readonly ILogger<TokenValidator> _logger;
        private readonly ISettingsProvider _settingsProvider;
        
        private readonly TokenValidationParameters _tokenValidationParameters;

        private const string AUTH_HEADER_NAME = "Authorization";
        private const string BEARER_PREFIX = "Bearer ";

        public TokenValidator(ILogger<TokenValidator> logger, ISettingsProvider settingsProvider)
        {
            _logger = logger;
            _settingsProvider = settingsProvider;
            IConfigurationManager<OpenIdConnectConfiguration> configurationManager = 
                new ConfigurationManager<OpenIdConnectConfiguration>(
                    $"{_settingsProvider.GetRequiredSetting("StsHost")}/tfp/{_settingsProvider.GetRequiredSetting("TenantDomain")}/{_settingsProvider.GetRequiredSetting("Policy")}/v2.0/.well-known/openid-configuration",
                    new OpenIdConnectConfigurationRetriever());
            OpenIdConnectConfiguration openIdConfig = configurationManager.GetConfigurationAsync(CancellationToken.None).GetAwaiter().GetResult();
            _tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                RequireSignedTokens = true,
                ValidateIssuer = true,
                ValidIssuer = $"{_settingsProvider.GetRequiredSetting("StsHost")}/{_settingsProvider.GetRequiredSetting("TenantId")}/v2.0/",
                ValidAudiences = new string[] { _settingsProvider.GetRequiredSetting("ValidAudience") },
                IssuerSigningKeys = openIdConfig.SigningKeys
            };
        }

        public TokenValidationResult ValidateToken(HttpRequest request)
        {
            {
                try
                {
                    // Get the token from the header
                    if (request != null &&
                        request.Headers.ContainsKey(AUTH_HEADER_NAME) &&
                        request.Headers[AUTH_HEADER_NAME].ToString().StartsWith(BEARER_PREFIX))
                    {
                        var token = request.Headers[AUTH_HEADER_NAME].ToString().Substring(BEARER_PREFIX.Length);

                        // Validate the token
                        var handler = new JwtSecurityTokenHandler();
                        var result = handler.ValidateToken(token, _tokenValidationParameters, out var securityToken);
                        _logger.LogInformation("Successfully validated token");
                        return TokenValidationResult.CreateSuccessResult(result);
                    }
                    else
                    {
                        _logger.LogInformation("No token was attached to the request");
                        return TokenValidationResult.CreateNoTokenResult();
                    }
                }
                catch (SecurityTokenExpiredException)
                {
                    _logger.LogWarning("Token in the request is expired");
                    return TokenValidationResult.CreateExpiredResult();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occured. Unable to validate token");
                    return TokenValidationResult.CreateErrorResult(ex);
                }
            }
        }
    }
}
