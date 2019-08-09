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
using AzFunctions.Tooling.Storage;

namespace AzFnAuthTest
{
    public class Function2
    {
        private readonly ICommandContext _commandContext;
        private readonly IRepository<User> _userRepository;

        public Function2(ICommandContext commandContext, IRepository<User> userRepo)
        {
            _commandContext = commandContext;
            _userRepository = userRepo;
        }
        [FunctionName("Function2")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            return await _commandContext.ExecuteActionAsync(async () => await DoAction(_commandContext));
        }

        private async Task<User> DoAction(ICommandContext context)
        {
            var user = new User(context.ObjectId.ToString(), "abc");
            user.Name = "Tanner12";
            user.C = new Complex()
            {
                Attribute = "hello",
                Table = "there"
            };

            return await _userRepository.UpsertDocumentAsync(user);
        }
    }
}
