using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; // DefaultHttpContext, HttpRequest/HttpResponse/HttpContext (Http.Abstractions)
using Microsoft.AspNetCore.Http.Internal; // DefaultHttpRequest/DefaultHttpResponse
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace Mth.Darts.Cricket.Api.Tests
{
    public class StartMatchTests
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _client;
        public StartMatchTests()
        {
            //IServiceCollection services = new IServiceCollection();
            services.AddHttpClient();
            IHttpClientFactory _httpClientFactory = new IHttpClientFactory();
        }

        [Fact]
        public async Task Post_TwoPlayerMatch_Success()
        {
            var apiPath = new PathString("https://mthcricket01api.azurewebsites.net/api/");
            var routePath = new PathString("/matches/");
            var path = apiPath.Add(routePath);
            var query = QueryString.Create(new[] {
                new KeyValuePair<string, string>("player", "P1"),
                new KeyValuePair<string, string>("player", "P2"),
                new KeyValuePair<string, string>("max_rounds", "20"),
                new KeyValuePair<string, string>("scoring_mode", "Standard")
            });


            //var client = _clientFactory.CreateClient();

            var request = new DefaultHttpRequest().Request;
            var requestFeature = request.HttpContext.Features.Get<IHttpRequestFeature>();
            requestFeature.QueryString = query;


            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethods.Post,
                Path = path,
                Query = query,
                Body = "",
            };
            var response = await client.PostAsync(request);

            var expected = File.ReadAllText(@"testData/TwoPlayerMatch_Expected.json");

            Assert.Equal(expected, response.Value);
        }
    }
}
