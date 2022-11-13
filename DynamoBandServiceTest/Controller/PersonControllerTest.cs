using DynamoBandService.Controllers;
using DynamoBandService.Models;
using DynamoBandService.Models.DTOs;
using DynamoBandService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DynamoBandServiceTest.Controller
{
    public class PersonControllerTest
    {
        private readonly Mock<IPersonService> _mockService;
        private readonly PersonController _controler;

        public PersonControllerTest()
        {
            _mockService = new Mock<IPersonService>();
            _controler = new PersonController(_mockService.Object);
        }

        [Fact]
        public async void GetById_CallServiceMethod_AtLeastOnce()
        {
            _mockService.Setup(service => service.GetPersonById(It.IsAny<string>()))
                .Verifiable();

            var result = await _controler.GetById(It.IsAny<string>());
            _mockService.Verify(service => service.GetPersonById(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public async void GetById_InexistentPerson_GetNotFound()
        {
            _mockService.Setup(service => service.GetPersonById(It.IsAny<string>()))
                .ThrowsAsync(new NullReferenceException());

            var result = await _controler.GetById(It.IsAny<string>()) as ObjectResult;
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async void GetById_GetPersonWithValidId_GetOkResponse()
        {
            _mockService.Setup(service => service.GetPersonById(It.IsAny<string>()))
                .ReturnsAsync(new Person())
                .Verifiable();

            var result = await _controler.GetById(It.IsAny<string>());
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetById_ErrorWhileGettingPerson_GetServerError()
        {
            _mockService.Setup(service => service.GetPersonById(It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            var result = await _controler.GetById(It.IsAny<string>()) as StatusCodeResult;
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async void GetAllPersons_CallServiceMethod_AtLeastOnce()
        {
            _mockService.Setup(service => service.GetAllPersons())
                .Verifiable();

            var result = await _controler.GetAllPersons();
            _mockService.Verify(service => service.GetAllPersons(), Times.AtLeastOnce);
        }

        [Fact]
        public async void GetAllPersons_GetValidPersons_GetOkResponse()
        {
            _mockService.Setup(service => service.GetAllPersons())
                .ReturnsAsync(new List<Person>() { new Person(), new Person() })
                .Verifiable();

            var result = await _controler.GetAllPersons();
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetAllPersons_ErrorWhileGettingArtistsByBand_GetServerError()
        {
            _mockService.Setup(service => service.GetAllPersons())
                .ThrowsAsync(new Exception())
                .Verifiable();

            var result = await _controler.GetAllPersons() as StatusCodeResult;
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async void CreatePerson_CallServiceMethod_AtLeastOnce()
        {
            _mockService.Setup(service => service.CreatePerson(It.IsAny<CreatePersonDTO>()))
                .Verifiable();

            var result = await _controler.CreatePerson(It.IsAny<CreatePersonDTO>());
            _mockService.Verify(service => service.CreatePerson(It.IsAny<CreatePersonDTO>()), Times.AtLeastOnce);
        }

        [Fact]
        public async void CreatePerson_CreateAPersonWithValidData_GetOkResponse()
        {
            _mockService.Setup(service => service.CreatePerson(It.IsAny<CreatePersonDTO>()))
                .ReturnsAsync(new Person());

            var result = await _controler.CreatePerson(It.IsAny<CreatePersonDTO>());
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void CreatePerson_InvalidDataReceived_GetBadRequest()
        {
            _mockService.Setup(service => service.CreatePerson(It.IsAny<CreatePersonDTO>()))
                .ThrowsAsync(new ArgumentNullException())
                .Verifiable();

            var result = await _controler.CreatePerson(It.IsAny<CreatePersonDTO>()) as ObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void CreatePerson_ErrorWhileCreatingPerson_GetServerError()
        {
            _mockService.Setup(service => service.CreatePerson(It.IsAny<CreatePersonDTO>()))
                .ThrowsAsync(new Exception());

            var result = await _controler.CreatePerson(It.IsAny<CreatePersonDTO>()) as StatusCodeResult;
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async void DeletePerson_CallServiceMethod_AtLeastOnce()
        {
            _mockService.Setup(service => service.DeletePerson(It.IsAny<string>()))
                .Verifiable();

            var result = await _controler.DeletePerson(It.IsAny<string>());
            _mockService.Verify(service => service.DeletePerson(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public async void DeletePerson_DeleteAPersonWithValidData_GetOkResponse()
        {
            _mockService.Setup(service => service.DeletePerson(It.IsAny<string>()))
                .ReturnsAsync(new Person());

            var result = await _controler.DeletePerson(It.IsAny<string>());
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void DeletePerson_InavlidSortID_GetBadRequest()
        {
            _mockService.Setup(service => service.DeletePerson(It.IsAny<string>()))
                .ThrowsAsync(new NullReferenceException());

            var result = await _controler.DeletePerson(It.IsAny<string>()) as ObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void DeletePerson_ErrorWhileDeletingPerson_GetServerError()
        {
            _mockService.Setup(service => service.DeletePerson(It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            var result = await _controler.DeletePerson(It.IsAny<string>()) as StatusCodeResult;
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async void UpdatePerson_CallServiceMethod_AtLeastOnce()
        {
            _mockService.Setup(service => service.UpdatePerson(It.IsAny<Person>()))
                .Verifiable();

            var result = await _controler.UpdatePerson(It.IsAny<Person>());
            _mockService.Verify(service => service.UpdatePerson(It.IsAny<Person>()), Times.AtLeastOnce);
        }

        [Fact]
        public async void UpdatePerson_UpdateAPersonWithValidData_GetOkResponse()
        {
            _mockService.Setup(service => service.UpdatePerson(It.IsAny<Person>()))
                .ReturnsAsync(new Person());

            var result = await _controler.UpdatePerson(It.IsAny<Person>());
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void UpdatePerson_InavlidDataReceived_GetBadRequest()
        {
            _mockService.Setup(service => service.UpdatePerson(It.IsAny<Person>()))
                .ThrowsAsync(new NullReferenceException());

            var result = await _controler.UpdatePerson(It.IsAny<Person>());
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void UpdatePerson_ErrorWhileUpdatingArtist_GetServerError()
        {
            _mockService.Setup(service => service.UpdatePerson(It.IsAny<Person>()))
                .ThrowsAsync(new Exception());

            var result = await _controler.UpdatePerson(It.IsAny<Person>()) as StatusCodeResult;
            Assert.Equal(500, result.StatusCode);
        }
    }
}
