using System;
using System.Collections.Generic;// List<string> players
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;// HttpRequest, HttpResponse, StatusCodes
using Microsoft.AspNetCore.Mvc; // IActionResult
using Microsoft.Azure.WebJobs;// FunctionName attribute
using Microsoft.Azure.WebJobs.Extensions.Http;// ReadAsStringAsync
using Microsoft.Extensions.Logging;// ILogger
using Mth.Darts.Cricket;// service we're using
using Newtonsoft.Json;// SerializeObject

namespace Mth.Darts.Cricket.Api
{
    public static class StartNewGame
    {
        [FunctionName("StartNewGame")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "matches/{matchGuid}/newgame")] 
            HttpRequest req,
            string matchGuid,
            ILogger log)
        {
            log.LogInformation($"StartNewGame function called for match {matchGuid}");

            // Expecting the full match in json format as the request body (at least until adding persistence)
            string body = await req.ReadAsStringAsync();
            Match match = JsonConvert.DeserializeObject<Match>(body);

            // Apply changes to the match object
            match.StartNewGame ();

            // Return the result
            var json = JsonConvert.SerializeObject(match, Formatting.Indented);
            return (ActionResult)new OkObjectResult(json);
        }
    }
}
