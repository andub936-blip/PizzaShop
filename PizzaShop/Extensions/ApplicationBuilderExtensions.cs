using PizzaShop.Middleware;

namespace PizzaShop.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseTiming(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestTimingMiddleware>();
        }

        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
