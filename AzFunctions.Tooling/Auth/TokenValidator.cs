using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace AzFunctions.Tooling.Auth
{
    public class TokenValidator : ITokenValidator
    {
        private readonly ILogger<TokenValidator> _logger;
        private readonly IConfiguration _config;
        
        private readonly TokenValidationParameters _tokenValidationParameters;

        private const string STS_HOST = "https://woodsplit.b2clogin.com";
        private const string VALID_AUDIENCE = "1fec7da5-70a7-4740-b920-67354eb810ef";
        private const string AUTH_HEADER_NAME = "Authorization";
        private const string BEARER_PREFIX = "Bearer ";
        private const string TENANT_SCOPE = "woodsplit.onmicrosoft.com";
        private const string POLICY = "B2C_1_SiUpIn";
        private const string TENANT_ID = "9f08c85b-4115-499d-b09d-4fe05f69c9cb";

        public TokenValidator(IConfiguration config, ILogger<TokenValidator> logger)
        {
            _logger = logger;
            _config = config;
            IConfigurationManager<OpenIdConnectConfiguration> configurationManager = 
                new ConfigurationManager<OpenIdConnectConfiguration>(
                    $"{STS_HOST}/tfp/{TENANT_SCOPE}/{POLICY}/v2.0/.well-known/openid-configuration",
                    new OpenIdConnectConfigurationRetriever());
            OpenIdConnectConfiguration openIdConfig = configurationManager.GetConfigurationAsync(CancellationToken.None).GetAwaiter().GetResult();
            _tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                RequireSignedTokens = true,
                ValidateIssuer = true,
                ValidIssuer = $"{STS_HOST}/{TENANT_ID}/v2.0/",
                ValidAudiences = new[] { VALID_AUDIENCE },
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
