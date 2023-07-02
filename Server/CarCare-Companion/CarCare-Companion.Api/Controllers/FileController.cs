namespace CarCare_Companion.Api.Controllers;

using CarCare_Companion.Common;
using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Status;
using CarCare_Companion.Infrastructure.Data.Models.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[Route("[controller]")]
[AllowAnonymous]
public class FileController : BaseController
{
	private readonly IImageService fileService;
	private readonly ILogger<FileController> logger;

    public FileController(IImageService fileService, ILogger<FileController> logger)
    {
        this.fileService = fileService;
		this.logger = logger;
    }


  //  [HttpPost]
  //  [Route("/File")]
  //  [Produces("application/json")]
  //  public async Task<IActionResult> UploadVehiclePicture([FromForm] IFormFile file)
  //  {
		//try
		//{
		//	if (file == null || file.Length == 0)
		//	{
		//		return StatusCode(400, new StatusErrorInformation(StatusResponses.BadRequest));
		//	}

		//	 var result = await fileService.UploadFileAsync(file, "car-care-companion-bucket");

		//	return Ok();

		//}
		//catch (Exception)
		//{

		//	throw;
		//}
  //  }
}
