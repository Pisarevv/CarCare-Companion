namespace CarCare_Companion.Core.Services;

using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using CarCare_Companion.Core.Contracts;
using CarCare_Companion.Core.Models.Ads;
using CarCare_Companion.Infrastructure.Data.Common;
using CarCare_Companion.Infrastructure.Data.Models.Ads;



public class AdService : IAdService
{
    private readonly IRepository repository;

    public AdService(IRepository repository)
    {
        this.repository = repository;
    }

    public async Task<ICollection<CarouselAdRequestModel>> GetFiveAsync()
    {
        return await repository.AllReadonly<CarouselAdModel>()
                               .Select(c => new CarouselAdRequestModel
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
