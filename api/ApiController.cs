using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SoundBoardPOC
{
    public class ApiController : Controller
    {
        private readonly ILogger _logger;
        public ApiController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ApiController>();
        }

        [HttpGet("api/honk")]
        [AllowAnonymous]
        public IActionResult Honk(string hi)
        {
            if (hi.Equals(string.Empty))
                return Content(string.Empty);
            return Content("data: Honk honk " + hi + "\n\n", "text/event-stream");
        }
    }
}