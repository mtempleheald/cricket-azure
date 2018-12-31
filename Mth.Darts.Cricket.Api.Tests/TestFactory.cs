using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace Mth.Darts.Cricket.Api.Tests
{
    public class TestFactory
    {
        public static DefaultHttpRequest CreateHttpRequest(Dictionary<string, StringValues> dictionary, Stream body)
        {
            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection(dictionary),
                Body = body
            };
            return request;
        }

        public static ILogger CreateLogger(LoggerTypes type = LoggerTypes.Null)
        {
            ILogger logger;

            if (type == LoggerTypes.List)
            {
                logger = new ListLogger();
            }
            else
            {
                logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            }

            return logger;
        }
    }
}