namespace CarCare_Companion.Core.Services;

using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Ads;
using CarCare_Companion.Infrastructure.Data.Common;
using CarCare_Companion.Infrastructure.Data.Models.Ads;


/// <summary>
/// The AdService is responsible for retrieving the ads to the client
/// </summary>
public class AdService : IAdService
{
    private readonly IRepository repository;

    public AdService(IRepository repository)
    {
        this.repository = repository;
    }

    /// <summary>
    /// Retrieves all the carousel ads 
    /// </summary>
    /// <returns>Collection of carousel ad models</returns>
    public async Task<ICollection<CarouselAdResponseModel>> GetAllAsync()
    {
        return await repository.AllReadonly<CarouselAdModel>()
                               .Select(c => new CarouselAdResponseModel
                               {
                                   Id = c.Id.ToString(),
                                   UserFirstName = c.UserFirstName,
                                   Description = c.Description,
                                   ReviewStars = c.StarsRating,
                               })
                               .ToListAsync();


    }


    /// <summary>
    /// Retrieves the carousel ads data
    /// </summary>
    /// <returns>Collection of carousel ad models</returns>
    public async Task<ICollection<CarouselAdResponseModel>> GetFiveAsync()
    {
        return await repository.AllReadonly<CarouselAdModel>()
                               .Select(c => new CarouselAdResponseModel
                               {
                                   Id = c.Id.ToString(),
                                   UserFirstName = c.UserFirstName,
                                   Description = c.Description,
                                   ReviewStars = c.StarsRating,
                               })
                               .Take(5)
                               .ToListAsync();


    }
}
