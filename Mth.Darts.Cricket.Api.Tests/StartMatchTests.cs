using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace Mth.Darts.Cricket.Api.Tests
{
    public class StartMatchTests
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]
        public async void TwoPlayerMatch_Success()
        {
            var expected = File.ReadAllText(@"testData/TwoPlayerMatch_initial.json");

            Dictionary<string, StringValues> reqParams = new Dictionary<string, StringValues>();
            reqParams.Add("scoring_mode", new StringValues("Standard"));
            reqParams.Add("max_rounds", new StringValues("0"));
            reqParams.Add("player", new StringValues(new string[] {"Van Gerwen", "Taylor"}));

            var request = TestFactory.CreateHttpRequest (reqParams, new MemoryStream());
            var response = (OkObjectResult)await StartMatch.Run(request, logger);
            
            Assert.Equal(expected, response.Value);
        }
    }
}
