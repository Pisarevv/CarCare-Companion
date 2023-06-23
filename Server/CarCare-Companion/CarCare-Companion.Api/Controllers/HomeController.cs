namespace CarCare_Companion.Api.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[Route("[controller]")]
public class HomeController : BaseController
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "Index")]
    public async Task<IActionResult> Index()
    {
        var data = new { message = " Test " };
        return Ok(data);
    }

}
