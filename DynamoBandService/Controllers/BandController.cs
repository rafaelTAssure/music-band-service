using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using DynamoBandService.Helpers;
using DynamoBandService.Models;
using DynamoBandService.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DynamoBandService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BandController : Controller
    {
        private readonly string BAND = "BAND";
        private readonly IDynamoDBContext _context;
        public BandController(IDynamoDBContext context)
        {
            _context = context;
        }

        [HttpGet("{sortId}")]
        public async Task<IActionResult> GetById(string sortId)
        {
            var band = await _context.LoadAsync<Band>(BAND, sortId);

            if (band == null)
            {
                return NotFound();
            }

            return Ok(band);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBands()
        {
            List<object> queryVal = new()
            {
                $"{BAND}#"
            };

            var bands = await _context.QueryAsync<Band>(BAND, QueryOperator.BeginsWith, queryVal).GetRemainingAsync();
            return Ok(bands);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBand(CreateBandDTO bandRequest)
        {
            if (string.IsNullOrEmpty(bandRequest.Name))
            {
                return BadRequest("Empty field");
            }

            var bandSortId = KeysHelper.BuildKey(BAND, bandRequest.Name);

            var band = await _context.LoadAsync<Band>(BAND, bandSortId);
            if (band != null)
            {
                return BadRequest($"Band with Id {bandSortId} Already Exists");
            }

            band = new Band()
            {
                Id = BAND,
                SortId = bandSortId,
                Name = bandRequest.Name,
                Genre = bandRequest.Genre
            };

            await _context.SaveAsync(band);

            return Ok(band);
        }

        [HttpDelete("{sortId}")]
        public async Task<IActionResult> DeleteBand(string sortId)
        {
            var band = await _context.LoadAsync<Band>(BAND, sortId);
            if (band == null)
            {
                return NotFound();
            }

            await _context.DeleteAsync(band);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBand(Band bandRequest)
        {
            var band = await _context.LoadAsync<Band>(BAND, bandRequest.SortId);
            if (band == null)
            {
                return NotFound("Element Not Found");
            }

            band.Genre = bandRequest.Genre;
            band.Name = bandRequest.Name;

            await _context.SaveAsync(band);
            return Ok(band);
        }
    }
}
