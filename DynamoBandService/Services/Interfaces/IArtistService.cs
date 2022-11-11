using DynamoBandService.Models;
using DynamoBandService.Models.DTOs;

namespace DynamoBandService.Services.Interfaces
{
    public interface IArtistService
    {
        Task<Artist> CreateArtist(CreateArtistDTO artistDto);
        Task<List<Artist>> GetArtistByBand(string bandSortId);
        Task<Artist> DeleteArtist(string sortId);
        Task<Artist> GetArtistById(string sortId);
        Task<Artist> UpdateArtist(Artist artistRequest);

    }
}
