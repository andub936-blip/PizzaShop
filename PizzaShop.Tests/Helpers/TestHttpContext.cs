using Microsoft.AspNetCore.Http;

namespace PizzaShop.Tests.Helpers
{
    public static class TestHttpContext
    {
        public static IHttpContextAccessor CreateAccessor()
        {
            var context = new DefaultHttpContext();
            context.Session = new TestSession();

            return new HttpContextAccessor
            {
                HttpContext = context
            };
        }
    }
}
