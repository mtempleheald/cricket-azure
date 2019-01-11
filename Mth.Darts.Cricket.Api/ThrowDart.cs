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
    public static class ThrowDart
    {
        [FunctionName("ThrowDart")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "matches/{matchGuid}/throw")] 
            HttpRequest req,
            string matchGuid,
            ILogger log)
        {
            log.LogInformation($"ThrowDart function called for match {matchGuid}");

            // Expecting the full match in json format as the request body (at least until adding persistence)
            string body = await req.ReadAsStringAsync();
            Match match = JsonConvert.DeserializeObject<Match>(body);

            // Expecting 2 parameters, bed and section, values optional in case of miss or non-scoring hit
            Section section;
            bool sectionHit = Enum.TryParse(req.Query["section"][0], true, out section);
            Section? effectiveSection = Enum.IsDefined(typeof(Section), section) ? section : (Section?)null;
            Bed bed;
            bool bedHit = Enum.TryParse(req.Query["bed"][0], true, out bed);
            Bed? effectiveBed = Enum.IsDefined(typeof(Bed), bed) ? bed : (Bed?)null;

            // Apply changes to the match object
            match.Throw (effectiveSection, effectiveBed);

            // Return the result
            var json = JsonConvert.SerializeObject(match, Formatting.Indented);
            return (ActionResult)new OkObjectResult(json);
        }
    }
}
