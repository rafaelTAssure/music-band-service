using DynamoBandService.Models;
using DynamoBandService.Models.DTOs;
using DynamoBandService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DynamoBandService.Controllers
{
    [Route("api/bands")]
    [ApiController]
    public class BandController : Controller
    {
        private readonly IBandService _bandService;
        public BandController(IBandService bandService)
        {
            _bandService = bandService;
        }

        [HttpGet("{sortId}")]
        public async Task<IActionResult> GetById(string sortId)
        {
            try
            {
                var band = await _bandService.GetBandById(sortId);

                if (band == null)
                {
                    return NotFound("Band not Found");
                }

                return Ok(band);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBands()
        {
            try
            {
                var bands = await _bandService.GetAllBands();
                return Ok(bands);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> CreateBand(CreateBandDTO bandRequest)
        {
            try
            {
                var band = await _bandService.CreateBand(bandRequest);
                return Ok(band);
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
        public async Task<IActionResult> DeleteBand(string sortId)
        {
            try
            {
                await _bandService.DeleteBand(sortId);
                return NoContent();
            }
            catch(NullReferenceException e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("Band not found");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
            
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBand(Band bandRequest)
        {
            try
            {
                var band = await _bandService.UpdateBand(bandRequest);
                return Ok(band);
            }
            catch(NullReferenceException e) 
            {
                Console.WriteLine(e.Message);
                return BadRequest("Band not found");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
        }
    }
}
