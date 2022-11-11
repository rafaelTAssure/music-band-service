using DynamoBandService.Models;
using DynamoBandService.Models.DTOs;

namespace DynamoBandService.Services.Interfaces
{
    public interface IBandService
    {
        Task<Band> GetBandById(string sortId);
        Task<List<Band>> GetAllBands();
        Task<Band> CreateBand(CreateBandDTO bandDto);
        Task<Band> DeleteBand(string sortId);
        Task<Band> UpdateBand(Band bandRequest);

    }
}
