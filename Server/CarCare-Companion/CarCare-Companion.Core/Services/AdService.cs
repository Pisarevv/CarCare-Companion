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
                                   StarsRating = c.StarsRating,
                               })
                               .ToListAsync();


    }

    /// <summary>
    /// Checks if an ad exists
    /// </summary>
    /// <param name="carouselAdId">The carousel ad identifier</param>
    /// <returns>A model containing the ad information</returns>
    public async Task<bool> DoesAdExistAsync(string carouselAdId)
    {
        return await repository.AllReadonly<CarouselAdModel>()
                             .Where(c => c.Id == Guid.Parse(carouselAdId))
                             .AnyAsync();
    }


    /// <summary>
    /// Retrieves the details about a carousel ad 
    /// </summary>
    /// <param name="carouselAdId">The carousel ad identifier</param>
    /// <returns>A model containing the ad information</returns>
    public async Task<CarouselAdResponseModel> GetDetailsAsync(string carouselAdId)
    {
        return await repository.AllReadonly<CarouselAdModel>()
                             .Where(c => c.Id == Guid.Parse(carouselAdId))
                             .Select(c => new CarouselAdResponseModel
                             {
                                 Id = c.Id.ToString(),
                                 UserFirstName = c.UserFirstName,
                                 Description = c.Description,
                                 StarsRating = c.StarsRating,
                             })
                             .FirstAsync();
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
                                   StarsRating = c.StarsRating,
                               })
                               .Take(5)
                               .ToListAsync();


    }
}
