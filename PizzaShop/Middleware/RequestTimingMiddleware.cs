using System.Diagnostics;

namespace PizzaShop.Middleware
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestTimingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                await _next(context);
            }
            finally
            {
                sw.Stop();
                Console.WriteLine(
                                $"{context.Request.Method} " +
                                $"{context.Request.Path} -> " +
                                $"{context.Response.StatusCode} " +
                                $"({sw.Elapsed.TotalMilliseconds} ms)");
            }
        }
    }
}
