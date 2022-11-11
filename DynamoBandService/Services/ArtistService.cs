using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using DynamoBandService.Helpers;
using DynamoBandService.Models;
using DynamoBandService.Models.DTOs;
using DynamoBandService.Repositories.Interfaces;
using DynamoBandService.Services.Interfaces;

namespace DynamoBandService.Services
{
    public class ArtistService : IArtistService
    {
        private readonly string ARTIST = "ARTIST";
        private readonly IRepository<Artist> _repository;

        public ArtistService(IRepository<Artist> repository)
        {
            _repository = repository;
        }
        public async Task<Artist> CreateArtist(CreateArtistDTO artistDto)
        {
            if (string.IsNullOrEmpty(artistDto.BandSortId))
            {
                throw new ArgumentException();
            }

            var id = Guid.NewGuid().ToString();
            var sortId = KeysHelper.BuildKey(artistDto.BandSortId, id);

            var artist = new Artist()
            {
                Id = ARTIST,
                SortId = sortId,
                Name = artistDto.Name,
                Email = artistDto.Email,
                DateOfBirth = artistDto.DateOfBirth,
                NickName = artistDto.NickName,
                Nationality = artistDto.Nationality,
                DebutYear = artistDto.DebutYear
            };

            await _repository.Save(artist);
            //await _context.SaveAsync(artist);

            return artist;
        }

        public async Task<Artist> DeleteArtist(string sortId)
        {
            var artist = await _repository.Load(ARTIST, sortId);
            //var artist = await _context.LoadAsync<Artist>(ARTIST, sortId);
            if (artist == null)
            {
                throw new NullReferenceException();
            }

            await _repository.Delete(artist);

            return artist;
        }

        public async Task<List<Artist>> GetArtistByBand(string bandSortId)
        {
            List<object> queryVal = new()
            {
                $"{bandSortId}#"
            };

            var artists = await _repository.Query(ARTIST, QueryOperator.BeginsWith, queryVal);
            //var artists = await _context.QueryAsync<Artist>(ARTIST, QueryOperator.BeginsWith, queryVal).GetRemainingAsync();

            return artists;
        }

        public Task<Artist> GetArtistById(string sortId)
        {
            return _repository.Load(ARTIST, sortId);
            //return _context.LoadAsync<Artist>(ARTIST, sortId);
        }

        public async Task<Artist> UpdateArtist(Artist artistRequest)
        {
            if (string.IsNullOrEmpty(artistRequest.SortId))
            {
                throw new ArgumentNullException();
            }

            var artist = await _repository.Load(ARTIST, artistRequest.SortId);
            //var artist = await _context.LoadAsync<Artist>(ARTIST, artistRequest.SortId);
            if (artist == null)
            {
                throw new NullReferenceException();
            }

            artist.Name = artistRequest.Name;
            artist.DateOfBirth = artistRequest.DateOfBirth;
            artist.DebutYear = artistRequest.DebutYear;
            artist.Email = artistRequest.Email;
            artist.NickName = artistRequest.NickName;
            artist.Nationality = artistRequest.Nationality;

            await _repository.Save(artist);
            //await _context.SaveAsync(artist);

            return artist;
        }
    }
}
