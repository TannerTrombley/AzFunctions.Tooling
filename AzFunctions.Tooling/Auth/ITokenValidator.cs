using Microsoft.AspNetCore.Http;

namespace AzFunctions.Tooling.Auth
{
    public interface ITokenValidator
    {
        TokenValidationResult ValidateToken(HttpRequest request);
    }
}
