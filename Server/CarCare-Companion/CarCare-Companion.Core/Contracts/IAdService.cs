namespace CarCare_Companion.Core.Contracts;

using CarCare_Companion.Core.Models.Ads;

public interface IAdService
{
    public Task<ICollection<CarouselAdResponseModel>> GetFiveAsync();

    public Task<ICollection<CarouselAdResponseModel>> GetAllAsync();

    public Task<bool> DoesAdExistAsync(string carouselAdId);

    public Task<CarouselAdResponseModel> GetDetailsAsync(string carouselAdId);

    public Task<CarouselAdResponseModel> EditAsync(string carouselAdId, CarouselAdFromRequestModel model);
    
}
