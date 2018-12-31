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
    public class ThrowDartTests
    {
        private const string initialBody = @"testData/TwoPlayerMatch_initial.json";
        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]
        public async void TwoPlayerMatch_HitSomething_Success()
        {
            Dictionary<string, StringValues> reqParams = new Dictionary<string, StringValues>();
            reqParams.Add("section", new StringValues("Twenty"));
            reqParams.Add("bed", new StringValues("Treble"));

            HttpRequest request;
            OkObjectResult response;
            using (FileStream fs = File.OpenRead(initialBody))
            {
                request = TestFactory.CreateHttpRequest(reqParams, fs);
                response = (OkObjectResult)await ThrowDart.Run(request, "2b35924a-1f53-487b-814d-035c38c841ed", logger);
            }
            var expected = File.ReadAllText(@"testData/TwoPlayerMatch_afterT20only.json");
            Assert.Equal(expected, response.Value);
        }
    }
}
