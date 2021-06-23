using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace League.API
{
    public class HttpLoggingHandler : DelegatingHandler
    {
        private const string LogCategory = "HTTP";
        private const string NoResponseBodyText = "[response body empty]";
        private const string LogFormat = "Request to '{0}'. Status code: {1}, Response Body: {2}";

        private readonly ILogger _logger;

        public HttpLoggingHandler(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(LogCategory);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            LogLevel level = (int)response.StatusCode switch
            {
                int c when c >= 500 => LogLevel.Error,
                int c when c >= 400 => LogLevel.Warning,
                _ => LogLevel.Debug
            };

            _logger.Log(
                level, LogFormat, request.RequestUri, response.StatusCode,
                response.Content != null
                    ? await response.Content?.ReadAsStringAsync()
                    : NoResponseBodyText);

            return response;
        }
    }
}
