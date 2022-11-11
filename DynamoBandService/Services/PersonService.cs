using Amazon.DynamoDBv2.DocumentModel;
using DynamoBandService.Helpers;
using DynamoBandService.Models;
using DynamoBandService.Models.DTOs;
using DynamoBandService.Repositories.Interfaces;
using DynamoBandService.Services.Interfaces;

namespace DynamoBandService.Services
{
    public class PersonService : IPersonService
    {
        private readonly string PERSON = "PERSON";
        private readonly IRepository<Person> _repository;
        public PersonService(IRepository<Person> repository)
        {
            _repository = repository;
        }

        public async Task<Person> CreatePerson(CreatePersonDTO personRequest)
        {

            if (string.IsNullOrEmpty(personRequest.Email))
            {
                throw new ArgumentNullException();
            }

            var sortGuidPart = Guid.NewGuid().ToString();
            var personSortId = KeysHelper.BuildKey(PERSON, sortGuidPart);

            var person = new Person()
            {
                Id = PERSON,
                SortId = personSortId,
                Email = personRequest.Email,
                Name = personRequest.Name,
                DateOfBirth = personRequest.DateOfBirth,
                Nationality = personRequest.Nationality,
            };

            await _repository.Save(person);
            //await _context.SaveAsync(person);

            return person;
        }

        public async Task<Person> DeletePerson(string sortId)
        {
            var person = await _repository.Load(PERSON, sortId);
            //var person = await _context.LoadAsync<Person>(PERSON, sortId);
            if (person == null)
            {
                throw new NullReferenceException();
            }

            await _repository.Delete(person);
            //await _context.DeleteAsync(person);

            return person;
        }

        public async Task<List<Person>> GetAllPersons()
        {
            List<object> queryVal = new()
            {
                $"{PERSON}#"
            };

            var persons = await _repository.Query(PERSON, QueryOperator.BeginsWith, queryVal);
            //var persons = await _context.QueryAsync<Person>(PERSON, QueryOperator.BeginsWith, queryVal).GetRemainingAsync();

            return persons;
        }

        public async Task<Person> GetPersonById(string sortId)
        {
            var person = await _repository.Load(PERSON, sortId);
            //var person = await _context.LoadAsync<Person>(PERSON, sortId);
            
            if (person == null)
            {
                throw new NullReferenceException();
            }

            return person;
        }

        public async Task<Person> UpdatePerson(Person personRequest)
        {
            if (string.IsNullOrEmpty(personRequest.SortId))
            {
                throw new ArgumentNullException();
            }
            var person = await _repository.Load(PERSON, personRequest.SortId);
            //var person = await _context.LoadAsync<Person>(PERSON, personRequest.SortId);
            if (person == null)
            {
                throw new NullReferenceException();
            }

            person.Name = personRequest.Name;
            person.Email = personRequest.Email;
            person.Nationality = personRequest.Nationality;
            person.DateOfBirth = personRequest.DateOfBirth;

            await _repository.Save(person);
            //await _context.SaveAsync(person);

            return person;
        }
    }
}
