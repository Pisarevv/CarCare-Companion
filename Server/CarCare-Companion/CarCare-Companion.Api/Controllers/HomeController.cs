using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CarCare_Companion.Api.Controllers
{
    [Route("[controller]")]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet(Name = "Index")]
        public async Task<IActionResult> Index()
        {
            var data = new { message = " Test " };
            return Ok(data);
        }

    }
}