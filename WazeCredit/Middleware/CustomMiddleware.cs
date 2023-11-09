using WazeCredit.Services.LifetimeExample;

namespace WazeCredit.Middleware
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;
        public CustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, 
            TransientService transientService,
            ScopedService scopedService,
            SingletonService singletonService)
        {
            httpContext.Items.Add("CustomMiddlewareTransient", $"Transient Middleware - {transientService.GetGuid()}");
            httpContext.Items.Add("CustomMiddlewareScoped", $"Scoped Middleware - {scopedService.GetGuid()}");
            httpContext.Items.Add("CustomMiddlewareSingleton", $"Singleton Middleware - {singletonService.GetGuid()}");

            await _next(httpContext);
        }
    }
}
