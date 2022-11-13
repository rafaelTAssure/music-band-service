using DynamoBandService.Controllers;
using DynamoBandService.Models;
using DynamoBandService.Models.DTOs;
using DynamoBandService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DynamoBandServiceTest.Controller
{
    public class ArtistControllerTest
    {
        private readonly Mock<IArtistService> _mockService;
        private readonly ArtistController _controler;
        public ArtistControllerTest()
        {
            _mockService = new Mock<IArtistService>();
            _controler = new ArtistController(_mockService.Object);
        }

        [Fact]
        public async void GetById_CallServiceMethod_AtLeastOnce()
        {
            _mockService.Setup(service => service.GetArtistById(It.IsAny<string>()))
                .Verifiable();

            var result = await _controler.GetById(It.IsAny<string>());
            _mockService.Verify(service => service.GetArtistById(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public async void GetById_InexistentArtist_GetNotFound()
        {
            _mockService.Setup(service => service.GetArtistById(It.IsAny<string>()))
                .ReturnsAsync((Artist)null)
                .Verifiable();

            var result = await _controler.GetById("") as ObjectResult;
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async void GetById_GetAValidArtist_GetOkResponse()
        {
            _mockService.Setup(service => service.GetArtistById(It.IsAny<string>()))
                .ReturnsAsync(new Artist())
                .Verifiable();

            var result = await _controler.GetById(It.IsAny<string>());
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetById_ErrorWhileGettingArtist_GetServerError()
        {
            _mockService.Setup(service => service.GetArtistById(It.IsAny<string>()))
                .ThrowsAsync(new Exception())
                .Verifiable();

            var result = await _controler.GetById(It.IsAny<string>()) as StatusCodeResult;
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async void GetAllArtistByBand_CallServiceMethod_AtLeastOnce()
        {
            _mockService.Setup(service => service.GetArtistByBand(It.IsAny<string>()))
                .Verifiable();

            var result = await _controler.GetAllArtistsByBand(It.IsAny<string>());
            _mockService.Verify(service => service.GetArtistByBand(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public async void GetAllArtistByBand_GetValidArtists_GetOkResponse()
        {
            _mockService.Setup(service => service.GetArtistByBand(It.IsAny<string>()))
                .ReturnsAsync(new List<Artist>() { new Artist(), new Artist()})
                .Verifiable();

            var result = await _controler.GetAllArtistsByBand(It.IsAny<string>());
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetAllArtistByBand_ErrorWhileGettingArtistsByBand_GetServerError()
        {
            _mockService.Setup(service => service.GetArtistByBand(It.IsAny<string>()))
                .ThrowsAsync(new Exception())
                .Verifiable();

            var result = await _controler.GetAllArtistsByBand(It.IsAny<string>()) as StatusCodeResult;
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async void CreateArtist_CallServiceMethod_AtLeastOnce()
        {
            _mockService.Setup(service => service.CreateArtist(It.IsAny<CreateArtistDTO>()))
                .Verifiable();

            var result = await _controler.CreateArtist(It.IsAny<CreateArtistDTO>());
            _mockService.Verify(service => service.CreateArtist(It.IsAny<CreateArtistDTO>()), Times.AtLeastOnce);
        }

        [Fact]
        public async void CreateArtist_CreateAnArtistWithValidData_GetOkResponse()
        {
            _mockService.Setup(service => service.CreateArtist(It.IsAny<CreateArtistDTO>()))
                .ReturnsAsync(new Artist());

            var result = await _controler.CreateArtist(It.IsAny<CreateArtistDTO>());
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void CreateArtist_InavlidDataReceived_GetBadRequest()
        {
            _mockService.Setup(service => service.CreateArtist(It.IsAny<CreateArtistDTO>()))
                .ThrowsAsync(new ArgumentException())
                .Verifiable();

            var result = await _controler.CreateArtist(It.IsAny<CreateArtistDTO>()) as ObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void CreateArtist_ErrorWhileCreatingArtist_GetServerError()
        {
            _mockService.Setup(service => service.CreateArtist(It.IsAny<CreateArtistDTO>()))
                .ThrowsAsync(new Exception());

            var result = await _controler.CreateArtist(It.IsAny<CreateArtistDTO>()) as StatusCodeResult;
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async void DeleteArtist_CallServiceMethod_AtLeastOnce()
        {
            _mockService.Setup(service => service.DeleteArtist(It.IsAny<string>()))
                .Verifiable();

            var result = await _controler.DeleteArtist(It.IsAny<string>());
            _mockService.Verify(service => service.DeleteArtist(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public async void DeleteArtist_DeleteAnArtistWithValidData_GetOkResponse()
        {
            _mockService.Setup(service => service.DeleteArtist(It.IsAny<string>()))
                .ReturnsAsync(new Artist());

            var result = await _controler.DeleteArtist(It.IsAny<string>());
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void DeleteArtist_InavlidSortID_GetBadRequest()
        {
            _mockService.Setup(service => service.DeleteArtist(It.IsAny<string>()))
                .ThrowsAsync(new NullReferenceException());

            var result = await _controler.DeleteArtist(It.IsAny<string>()) as ObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void DeleteArtist_ErrorWhileDeletingArtist_GetServerError()
        {
            _mockService.Setup(service => service.DeleteArtist(It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            var result = await _controler.DeleteArtist(It.IsAny<string>()) as StatusCodeResult;
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async void UpdateArtist_CallServiceMethod_AtLeastOnce()
        {
            _mockService.Setup(service => service.UpdateArtist(It.IsAny<Artist>()))
                .Verifiable();

            var result = await _controler.UpdateArtist(It.IsAny<Artist>());
            _mockService.Verify(service => service.UpdateArtist(It.IsAny<Artist>()), Times.AtLeastOnce);
        }

        [Fact]
        public async void UpdateArtist_UpdateAnArtistWithValidData_GetOkResponse()
        {
            _mockService.Setup(service => service.UpdateArtist(It.IsAny<Artist>()))
                .ReturnsAsync(new Artist());

            var result = await _controler.UpdateArtist(It.IsAny<Artist>());
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void UpdateArtist_InavlidDataReceived_GetBadRequest()
        {
            _mockService.Setup(service => service.UpdateArtist(It.IsAny<Artist>()))
                .ThrowsAsync(new NullReferenceException());

            var result = await _controler.UpdateArtist(It.IsAny<Artist>());
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void UpdateArtist_ErrorWhileUpdatingArtist_GetServerError()
        {
            _mockService.Setup(service => service.UpdateArtist(It.IsAny<Artist>()))
                .ThrowsAsync(new Exception());

            var result = await _controler.UpdateArtist(It.IsAny<Artist>()) as StatusCodeResult;
            Assert.Equal(500, result.StatusCode);
        }
    }
}
