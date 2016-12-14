using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SoundBoardPOC.Pages.Reactions
{
    public class ReactionsController : Controller
    {
        private readonly IHostingEnvironment _env;
        private readonly IAntiforgery _antiforgery;
        public ReactionsController(IHostingEnvironment env, IAntiforgery antiforgery)
        {
            _env = env;
            _antiforgery = antiforgery;
        }

        public ActionResult Index()
        {
            // Send the request token as a JavaScript-readable cookie
            var tokens = _antiforgery.GetAndStoreTokens(Request.HttpContext);
            Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions { HttpOnly = false });
            ViewData["page:title"] = "Reactions!";

            return View();
        }
    }
}