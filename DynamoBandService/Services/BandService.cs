using Amazon.DynamoDBv2.DocumentModel;
using DynamoBandService.Helpers;
using DynamoBandService.Models;
using DynamoBandService.Models.DTOs;
using DynamoBandService.Repositories.Interfaces;
using DynamoBandService.Services.Interfaces;

namespace DynamoBandService.Services
{
    public class BandService : IBandService
    {
        private readonly string BAND = "BAND";
        private readonly IRepository<Band> _repository;
        public BandService(IRepository<Band> repository)
        {
            _repository = repository;
        }

        public Task<Band> GetBandById(string sortId)
        {
            return _repository.Load(BAND, sortId);
            //return _context.LoadAsync<Band>(BAND, sortId);
        }

        public async Task<List<Band>> GetAllBands()
        {
            List<object> queryVal = new()
            {
                $"{BAND}#"
            };

            var bands = await _repository.Query(BAND, QueryOperator.BeginsWith, queryVal);
            //var bands = await _context.QueryAsync<Band>(BAND, QueryOperator.BeginsWith, queryVal).GetRemainingAsync();

            return bands;
        }

        public async Task<Band> CreateBand(CreateBandDTO bandDto)
        {
            if (string.IsNullOrEmpty(bandDto.Name))
            {
                throw new ArgumentException();
            }

            var id = Guid.NewGuid().ToString();
            var bandSortId = KeysHelper.BuildKey(BAND, id);

            var band = new Band()
            {
                Id = BAND,
                SortId = bandSortId,
                Name = bandDto.Name,
                Genre = bandDto.Genre
            };

            await _repository.Save(band);
            //await _context.SaveAsync(band);

            return band;
        }

        public async Task<Band> DeleteBand(string sortId)
        {
            var band = await _repository.Load(BAND, sortId);
            //var band = await _context.LoadAsync<Band>(BAND, sortId);
            if (band == null)
            {
                throw new NullReferenceException();
            }

            await _repository.Delete(band);
            //await _context.DeleteAsync(band);

            return band;
        }

        public async Task<Band> UpdateBand(Band bandRequest)
        {
            if (string.IsNullOrEmpty(bandRequest.SortId))
            {
                throw new ArgumentNullException();
            }

            var band = await _repository.Load(BAND, bandRequest.SortId);
            //var band = await _context.LoadAsync<Band>(BAND, bandRequest.SortId);
            if (band == null)
            {
                throw new NullReferenceException();
            }

            band.Genre = bandRequest.Genre;
            band.Name = bandRequest.Name;

            await _repository.Save(band);
            //await _context.SaveAsync(band);

            return band;
        }
    }
}
