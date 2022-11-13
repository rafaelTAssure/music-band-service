using DynamoBandService.Models;
using DynamoBandService.Models.DTOs;
using DynamoBandService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DynamoBandService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : Controller
    {
        private readonly IPersonService _personService;
        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet("{sortId}")]
        public async Task<IActionResult> GetById(string sortId)
        {
            try
            {
                var person = await _personService.GetPersonById(sortId);
                return Ok(person);
            }
            catch(NullReferenceException e)
            {
                Console.WriteLine(e.Message);
                return NotFound("Person not found");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPersons()
        {
            try
            {
                var persons = await _personService.GetAllPersons();
                return Ok(persons);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreatePerson(CreatePersonDTO personRequest)
        {
            try
            {
                var person = await _personService.CreatePerson(personRequest);
                return Ok(person);
            } 
            catch(ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("Email is needed");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
        }

        [HttpDelete("{sortId}")]
        public async Task<IActionResult> DeletePerson(string sortId)
        {
            try
            {
                await _personService.DeletePerson(sortId);
                return NoContent();
            }
            catch(NullReferenceException e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("Person not found, already deleted");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePerson(Person personRequest)
        {
            try
            {
                var person = await _personService.UpdatePerson(personRequest);
                return Ok(person);
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("Person not found");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
        }
    }
}
