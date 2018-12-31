using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace Mth.Darts.Cricket.Api.Tests
{
    public class StartNewGameTests
    {
        private const string initialBody = @"testData/TwoPlayerMatch_gamecomplete_cutdown.json";
        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]
        public async void TwoPlayerMatch_StartNewGame_Success()
        {
            Dictionary<string, StringValues> reqParams = new Dictionary<string, StringValues>();

            HttpRequest request;
            OkObjectResult response;
            using (FileStream fs = File.OpenRead(initialBody))
            {
                request = TestFactory.CreateHttpRequest(reqParams, fs);
                response = (OkObjectResult)await StartNewGame.Run(request, "2b35924a-1f53-487b-814d-035c38c841ed", logger);
            }
            var expected = File.ReadAllText(@"testData/TwoPlayerMatch_newgamestarted.json");
            Assert.Equal(expected, response.Value);
        }
    }
}
