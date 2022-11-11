using DynamoBandService.Models;
using DynamoBandService.Models.DTOs;

namespace DynamoBandService.Services.Interfaces
{
    public interface IPersonService
    {
        Task<Person> CreatePerson(CreatePersonDTO personRequest);
        Task<Person> DeletePerson(string sortId);
        Task<List<Person>> GetAllPersons();
        Task<Person> GetPersonById(string sortId);
        Task<Person> UpdatePerson(Person personRequest);

    }
}
