using Microsoft.AspNetCore.Mvc;


namespace CarCare_Companion.Api.Controllers
{
   
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult Index()
        {

            return View();
        }

    }
}