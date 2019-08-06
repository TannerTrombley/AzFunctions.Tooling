using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using AzFunctions.Tooling.Context;
using AzFunctions.Tooling.Auth;

namespace AzFnAuthTest
{
    public class Function1
    {

        private readonly ICommandContext _commandContext;

        public Function1(ICommandContext commandContext)
        {
            _commandContext = commandContext;
        }

        [FunctionName("Function1")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log, ExecutionContext context)
        {
            var config = new ConfigurationBuilder()
             .SetBasePath(context.FunctionAppDirectory)
             .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
             .AddEnvironmentVariables()
             .Build();

            if (_commandContext.AuthenticationResult.Status != TokenValidationStatus.Success)
            {
                return new UnauthorizedResult();
            }
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, and {_commandContext.ObjectId}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
