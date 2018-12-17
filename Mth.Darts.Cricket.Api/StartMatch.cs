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
    public static class StartMatch
    {
        [FunctionName("StartMatch")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("StartMatch triggered.  RequestUri={req.RequestUri}");
            
            ScoringMode scoringMode;
            Enum.TryParse(req.Query["scoring_mode"], out scoringMode);
            int maxRounds = int.Parse(req.Query["max_rounds"][0]);
            List<string> players = new List<string>(req.Query["player"]);

            Match match = new Match (players, scoringMode, maxRounds);
            var json = JsonConvert.SerializeObject (match, Formatting.Indented);

            dynamic body = await req.ReadAsStringAsync(); // Microsoft.Azure.WebJobs.Extensions.Http;
            log.LogInformation($"Request body: {body}");

            return (ActionResult)new OkObjectResult(json);
        }
    }
}
