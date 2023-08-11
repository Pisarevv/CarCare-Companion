namespace CarCare_Companion.Core.Contracts;

using CarCare_Companion.Core.Models.Ads;

public interface IAdService
{
    /// <summary>
    /// Fetches the top five Carousel Ads asynchronously.
    /// </summary>
    /// <returns>A collection of up to five CarouselAdResponseModels.</returns>
    public Task<ICollection<CarouselAdResponseModel>> GetFiveAsync();

    /// <summary>
    /// Retrieves all Carousel Ads asynchronously.
    /// </summary>
    /// <returns>A collection of all CarouselAdResponseModels.</returns>
    public Task<ICollection<CarouselAdResponseModel>> GetAllAsync();

    /// <summary>
    /// Checks if a specific Carousel Ad exists based on its ID asynchronously.
    /// </summary>
    /// <param name="carouselAdId">The ID of the Carousel Ad to check.</param>
    /// <returns>True if the Carousel Ad exists, otherwise false.</returns>
    public Task<bool> DoesAdExistAsync(string carouselAdId);

    /// <summary>
    /// Fetches the details of a specific Carousel Ad based on its ID asynchronously.
    /// </summary>
    /// <param name="carouselAdId">The ID of the Carousel Ad to retrieve.</param>
    /// <returns>The details of the Carousel Ad as a CarouselAdResponseModel.</returns>
    public Task<CarouselAdResponseModel> GetDetailsAsync(string carouselAdId);

    /// <summary>
    /// Edits the details of a specific Carousel Ad based on its ID asynchronously.
    /// </summary>
    /// <param name="carouselAdId">The ID of the Carousel Ad to edit.</param>
    /// <param name="model">The model containing the updated details for the Carousel Ad.</param>
    /// <returns>The updated details of the Carousel Ad as a CarouselAdResponseModel.</returns>
    public Task<CarouselAdResponseModel> EditAsync(string carouselAdId, CarouselAdFromRequestModel model);
}



