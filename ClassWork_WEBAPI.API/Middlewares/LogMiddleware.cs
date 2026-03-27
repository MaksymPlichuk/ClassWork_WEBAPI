using System.Diagnostics;

namespace ClassWork_WEBAPI.API.Middlewares
{
    public class LogMiddleware
    {
        private RequestDelegate _next;
        private ILogger<LogMiddleware> _logger;

        public LogMiddleware(RequestDelegate next, ILogger<LogMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var requestHeaders = new List<string>();

            foreach (var h in context.Request.Headers)
            {
                requestHeaders.Add($"{h.Key} -- {h.Value}");
            }

            _logger.LogInformation("REQUEST START\n" +
                $"Method: {context.Request.Method}\nPath: {context.Request.Path}"
                + $"Query: {context.Request.Query}\n{string.Join("\n", requestHeaders)}");

            _logger.LogInformation(1000, $"Recieved: {DateTime.Now}\n");

            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();
                var responseHeaders = new List<string>();

                foreach (var h in context.Response.Headers)
                {
                    responseHeaders.Add($"{h.Key} -- {h.Value}");
                }

                _logger.LogInformation("RESPONSE END\n" + $"{string.Join("\n", responseHeaders)}");
                _logger.LogInformation(1000, $"Ended: {DateTime.Now}\n Time Elapsed: {stopwatch.ElapsedMilliseconds} ms\n");
            }        
        }
    }
}
