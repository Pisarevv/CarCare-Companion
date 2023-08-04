namespace CarCare_Companion.Core.Services;

using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Ads;
using CarCare_Companion.Infrastructure.Data.Common;
using CarCare_Companion.Infrastructure.Data.Models.Ads;
using Microsoft.Extensions.Caching.Memory;

using static Common.CacheKeysAndDurations.CarouselAds;


/// <summary>
/// The AdService is responsible for retrieving the ads to the client
/// </summary>
public class AdService : IAdService
{
    private readonly IRepository repository;
    private readonly IMemoryCache memoryCache;

    public AdService(IRepository repository, IMemoryCache memoryCache)
    {
        this.repository = repository;
        this.memoryCache = memoryCache;
    }


    /// <summary>
    /// Retrieves all the carousel ads 
    /// </summary>
    /// <returns>Collection of carousel ad models</returns>
    public async Task<ICollection<CarouselAdResponseModel>> GetAllAsync()
    {
        ICollection<CarouselAdResponseModel>? allCarouselAds =
            this.memoryCache.Get<ICollection<CarouselAdResponseModel>>(CarouselAdsCacheKey);

        if (allCarouselAds == null)
        {
            allCarouselAds = await repository.AllReadonly<CarouselAdModel>()
                               .Select(c => new CarouselAdResponseModel
                               {
                                   Id = c.Id.ToString(),
                                   UserFirstName = c.UserFirstName,
                                   Description = c.Description,
                                   StarsRating = c.StarsRating,
                               })
                               .ToListAsync();

            MemoryCacheEntryOptions cacheOptions   = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(CarouselAdsCacheDurationMinutes));

            this.memoryCache.Set(CarouselAdsCacheKey, allCarouselAds, cacheOptions);
        }

        return allCarouselAds;
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
    /// Edits a carousel ad
    /// </summary>
    /// <param name="carouselAdId">The carousel ad identifier</param>
    /// <param name="model">The model containing the carousel ad data</param>
    public async Task EditAsync(string carouselAdId, CarouselAdFromRequestModel model)
    {
        CarouselAdModel carouselAdToEdit = await repository.GetByIdAsync<CarouselAdModel>(Guid.Parse(carouselAdId));

        carouselAdToEdit.UserFirstName = model.UserFirstName;
        carouselAdToEdit.Description = model.Description;
        carouselAdToEdit.StarsRating = model.StarsRating;
        carouselAdToEdit.ModifiedOn = DateTime.UtcNow;

        await repository.SaveChangesAsync();

        this.memoryCache.Remove(CarouselAdsCacheKey);
    }


    /// <summary>
    /// Retrieves the carousel ads data
    /// </summary>
    /// <returns>Collection of carousel ad models</returns>
    public async Task<ICollection<CarouselAdResponseModel>> GetFiveAsync()
    {

        ICollection<CarouselAdResponseModel>? fiveCarouselAds =
           this.memoryCache.Get<ICollection<CarouselAdResponseModel>>(CarouselAdsCacheKey);

        if (fiveCarouselAds == null)
        {
            fiveCarouselAds = await repository.AllReadonly<CarouselAdModel>()
                               .Select(c => new CarouselAdResponseModel
                               {
                                   Id = c.Id.ToString(),
                                   UserFirstName = c.UserFirstName,
                                   Description = c.Description,
                                   StarsRating = c.StarsRating,
                               })
                               .Take(5)
                               .ToListAsync();

            MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(CarouselAdsCacheDurationMinutes));

            this.memoryCache.Set(CarouselAdsCacheKey, fiveCarouselAds, cacheOptions);
        }

        return fiveCarouselAds;

    }
}
