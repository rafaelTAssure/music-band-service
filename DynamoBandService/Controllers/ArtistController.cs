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
    public class ArtistController : Controller
    {
        private readonly string ARTIST = "ARTIST";
        private readonly IDynamoDBContext _context;

        public ArtistController(IDynamoDBContext context)
        {
            _context = context;
        }

        [HttpGet("{sortId}")]
        public async Task<IActionResult> GetById(string sortId)
        {
            var artist = await _context.LoadAsync<Artist>(ARTIST, sortId);

            if (artist == null)
            {
                return NotFound();
            }

            return Ok(artist);
        }

        [HttpGet("artist-by-band/{bandSortId}")]
        public async Task<IActionResult> GetAllArtistsByBand(string bandSortId)
        {
            List<object> queryVal = new()
            {
                $"{bandSortId}#"
            };

            var artists = await _context.QueryAsync<Artist>(ARTIST, QueryOperator.BeginsWith, queryVal).GetRemainingAsync();
            return Ok(artists);
        }

        // POST: ArtistController/Create
        [HttpPost]
        public async Task<IActionResult> CreateArtist(CreateArtistDTO artistRequest)
        {
            if (string.IsNullOrEmpty(artistRequest.BandSortId))
            {
                return BadRequest("BandSortId is empty");
            }

            Guid id = Guid.NewGuid();
            var sortId = KeysHelper.BuildKey(artistRequest.BandSortId, id.ToString());

            var artist = new Artist()
            {
                Id = ARTIST,
                SortId = sortId,
                Name = artistRequest.Name,
                Email = artistRequest.Email,
                DateOfBirth = artistRequest.DateOfBirth,
                NickName = artistRequest.NickName,
                Nationality = artistRequest.Nationality,
                DebutYear = artistRequest.DebutYear
            };

            await _context.SaveAsync(artist);

            return Ok(artist);
        }

        [HttpDelete("{sortId}")]
        public async Task<IActionResult> DeleteArtist(string sortId)
        {
            var artist = await _context.LoadAsync<Artist>(ARTIST, sortId);
            if (artist == null)
            {
                return NotFound();
            }

            await _context.DeleteAsync(artist);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateArtist(Artist artistRequest)
        {
            var artist = await _context.LoadAsync<Artist>(ARTIST, artistRequest.SortId);
            if (artist == null)
            {
                return NotFound("Artist Not Found");
            }

            artist.Name = artistRequest.Name;
            artist.DateOfBirth = artistRequest.DateOfBirth;
            artist.DebutYear = artistRequest.DebutYear;
            artist.Email = artistRequest.Email;
            artist.NickName = artistRequest.NickName;
            artist.Nationality = artistRequest.Nationality;

            await _context.SaveAsync(artist);
            return Ok(artist);
        }
    }
}
