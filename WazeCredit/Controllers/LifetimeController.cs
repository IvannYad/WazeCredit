using Microsoft.AspNetCore.Mvc;
using WazeCredit.Services.LifetimeExample;

namespace WazeCredit.Controllers
{
    public class LifetimeController : Controller
    {
        private readonly TransientService _transientService;
        private readonly ScopedService _scopedService;
        private readonly SingletonService _singletonService;

        public LifetimeController(TransientService transientService,
            ScopedService scopedService,
            SingletonService singletonService)
        {
            _transientService = transientService;
            _scopedService = scopedService;
            _singletonService = singletonService;
        }
        public IActionResult Index()
        {
            var messages = new List<string>()
            {
                HttpContext.Items["CustomMiddlewareTransient"]!.ToString()!,
                $"Transient Controller - {_transientService.GetGuid()}",
                HttpContext.Items["CustomMiddlewareScoped"]!.ToString()!,
                $"Scoped Controller - {_transientService.GetGuid()}",
                HttpContext.Items["CustomMiddlewareSingleton"]!.ToString()!,
                $"Singleton Controller - {_transientService.GetGuid()}",
            };

            return View(messages);
        }
    }
}
