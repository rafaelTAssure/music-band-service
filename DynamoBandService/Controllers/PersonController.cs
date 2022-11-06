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
    public class PersonController : Controller
    {
        private readonly string PERSON = "PERSON";
        private readonly IDynamoDBContext _context;
        public PersonController(IDynamoDBContext context)
        {
            _context = context;
        }

        [HttpGet("{sortId}")]
        public async Task<IActionResult> GetById(string sortId)
        {
            var person = await _context.LoadAsync<Person>(PERSON, sortId);

            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPersons()
        {
            List<object> queryVal = new()
            {
                $"{PERSON}#"
            };

            var persons = await _context.QueryAsync<Person>(PERSON, QueryOperator.BeginsWith, queryVal).GetRemainingAsync();
            return Ok(persons);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePerson(CreatePersonDTO personRequest)
        {
            if (string.IsNullOrEmpty(personRequest.Email))
            {
                return BadRequest("An e-mail is required");
            }

            var personSortId = KeysHelper.BuildKey(PERSON, personRequest.Email);
            var person = await _context.LoadAsync<Person>(PERSON, personSortId);
            if (person != null)
            {
                return BadRequest($"Person with Id {personSortId} Already Exists");
            }

            person = new Person()
            {
                Id = PERSON,
                SortId = personSortId,
                Email = personRequest.Email,
                Name = personRequest.Name,
                DateOfBirth = personRequest.DateOfBirth,
                Nationality = personRequest.Nationality,
            };

            await _context.SaveAsync(person);

            return Ok(person);
        }

        [HttpDelete("{sortId}")]
        public async Task<IActionResult> DeletePerson(string sortId)
        {
            var person = await _context.LoadAsync<Person>(PERSON, sortId);
            if (person == null)
            {
                return NotFound();
            }

            await _context.DeleteAsync(person);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePerson(Person personRequest)
        {
            var person = await _context.LoadAsync<Person>(PERSON, personRequest.SortId);
            if (person == null)
            {
                return NotFound("Element Not Found");
            }

            person.Name = personRequest.Name;
            person.Email = personRequest.Email;
            person.Nationality = personRequest.Nationality;
            person.DateOfBirth = personRequest.DateOfBirth;

            await _context.SaveAsync(person);
            return Ok(person);
        }
    }
}
