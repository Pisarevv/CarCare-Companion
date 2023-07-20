namespace CarCare_Companion.Api.Controllers;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Services;
using Microsoft.AspNetCore.Mvc;


/// <summary>
/// The service records controller handles tax records related operations
/// </summary>
[Route("[controller]")]
public class TaxRecordsController : BaseController
{
    private readonly ITaxRecordsService taxRecordsService;
    private readonly IVehicleService vehicleService;
    private readonly ILogger<TaxRecordsService> logger;

    public TaxRecordsController(ITaxRecordsService taxRecordsService, IVehicleService vehicleService, ILogger<TaxRecordsService> logger)
    {
        this.taxRecordsService = taxRecordsService;
        this.vehicleService = vehicleService;
        this.logger = logger;
    }
}
