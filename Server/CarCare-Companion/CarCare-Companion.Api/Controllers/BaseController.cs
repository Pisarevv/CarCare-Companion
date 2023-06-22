namespace CarCare_Companion.Api.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
public abstract class BaseController : ControllerBase
{
}
