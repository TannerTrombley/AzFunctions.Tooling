using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AzFunctions.Tooling.Context;

namespace AzFnAuthTest
{
    public class Function2
    {
        private readonly ICommandContext _commandContext;

        public Function2(ICommandContext commandContext)
        {
            _commandContext = commandContext;
        }
        [FunctionName("Function2")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            return await _commandContext.ExecuteActionAsync(async () => await DoAction());
        }

        private async Task<string> DoAction()
        {
            return await Task.FromResult("ABC123");
        }
    }
}
