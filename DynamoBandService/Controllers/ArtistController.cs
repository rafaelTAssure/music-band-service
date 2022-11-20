using DynamoBandService.Models;
using DynamoBandService.Models.DTOs;
using DynamoBandService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DynamoBandService.Controllers
{
    [Route("api/artists")]
    [ApiController]
    public class ArtistController : Controller
    {
        private readonly IArtistService _artistService;

        public ArtistController(IArtistService artistService)
        {
            _artistService = artistService;
        }

        [HttpGet("{sortId}")]
        public async Task<IActionResult> GetById(string sortId)
        {
            try
            {
                var artist = await _artistService.GetArtistById(sortId);

                if (artist == null)
                {
                    return NotFound("Artist not found");
                }

                return Ok(artist);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("band/{bandSortId}")]
        public async Task<IActionResult> GetAllArtistsByBand(string bandSortId)
        {
            try
            {
                var artists = await _artistService.GetArtistByBand(bandSortId);
                return Ok(artists);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateArtist(CreateArtistDTO artistRequest)
        {
            try
            {
                var artist = await _artistService.CreateArtist(artistRequest);
                return Ok(artist);
            }
            catch(ArgumentException e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("Required field is empty");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
            
        }

        [HttpDelete("{sortId}")]
        public async Task<IActionResult> DeleteArtist(string sortId)
        {
            try
            {
                await _artistService.DeleteArtist(sortId);
                return NoContent();
            }
            catch(NullReferenceException e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("Required field is empty");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
            
        }

        [HttpPut]
        public async Task<IActionResult> UpdateArtist(Artist artistRequest)
        {
            try
            {
                var artist = await _artistService.UpdateArtist(artistRequest);
                return Ok(artist);
            }
            catch(NullReferenceException e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("Artist does not exist");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
            
        }
    }
}
