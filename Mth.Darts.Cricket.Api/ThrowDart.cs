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
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "matches/{matchGuid}/throw")] HttpRequest req,
            ILogger log)
        {
            // Expecting the full match in json format as the request body (at least until adding persistence)
            string body = await req.ReadAsStringAsync();
            Match match = JsonConvert.DeserializeObject<Match>(body);

            // Expecting 2 parameters, bed and section, values optional in case of miss
            Section section;
            bool sectionHit = Enum.TryParse(req.Query["section"][0], out section);
            Bed bed;
            bool bedHit = Enum.TryParse(req.Query["bed"][0], out bed);

            // Apply changes to the match object
            match.Throw (section, bed);

            // Return the result
            var json = JsonConvert.SerializeObject(match, Formatting.Indented);
            return (ActionResult)new OkObjectResult(json);
        }
    }
}
