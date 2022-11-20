using DynamoBandService.Controllers;
using DynamoBandService.Models;
using DynamoBandService.Models.DTOs;
using DynamoBandService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DynamoBandServiceTest.Controller
{
    public class BandControllerTest
    {
        private readonly Mock<IBandService> _mockService;
        private readonly BandController _controler;

        public BandControllerTest()
        {
            _mockService = new Mock<IBandService>();
            _controler = new BandController(_mockService.Object);

            _mockService.Setup(repo => repo.GetAllBands())
                .ReturnsAsync(new List<Band>() { new Band(), new Band() })
                .Verifiable();
        }

        [Fact]
        public async void GetAllBands_WhenCalled_GetAnOKResponse()
        {
            var result = await _controler.GetAllBands();
            Assert.IsType<OkObjectResult>(result as OkObjectResult);
        }

        [Fact]
        public async void GetAllBands_WhenCalled_ShouldExecuteAtLeastOnce()
        {
            OkObjectResult? result = await _controler.GetAllBands() as OkObjectResult;

            _mockService.Verify(repo => repo.GetAllBands(), Times.AtLeastOnce);
        }

        //this test is more for the service
        /*        [Fact]
                public async void GetAllBands_WhenCalled_ReturnsAList()
                {
                    OkObjectResult? result = await _controler.GetAllBands() as OkObjectResult;
                    var bands = Assert.IsType<List<Band>>(result == null ?  new List<Band>(): result.Value);

                    Assert.Equal(2, bands.Count);
                }*/

        [Fact]
        public async void GetById_CallServiceMethod_AtLeastOnce()
        {
            var sortId = "ab2bd817-98cd-4cf3-a80a-53ea0cd9c200";

            _mockService.Setup(service => service.GetBandById(It.IsAny<string>()))
                .ReturnsAsync(new Band())
                .Verifiable();

            var result = await _controler.GetById(sortId);
            _mockService.Verify(repo => repo.GetBandById(sortId), Times.AtLeastOnce);
        }

        [Fact]
        public async void GetById_WithInvalidSortId_GetNotFoundResponse()
        {
            var invalidSortId = "";
            _mockService.Setup(repo => repo.GetBandById(It.IsAny<string>()))
                .ReturnsAsync((Band)null)
                .Verifiable();

            var result = await _controler.GetById(invalidSortId) as ObjectResult;
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async void GetById_WithValidSortId_GetOkResponse()
        {
            var validSortId = "BAND#7156fdb6-1d2d-41f4-af32-c4173bb1f336";
            _mockService.Setup(repo => repo.GetBandById(It.IsAny<string>()))
                .ReturnsAsync(new Band())
                .Verifiable();

            var result = await _controler.GetById(validSortId) as ObjectResult;
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void CreateBand_CallServiceMethod_AtLeastOnce()
        {
            _mockService.Setup(service => service.CreateBand(It.IsAny<CreateBandDTO>()))
                .ReturnsAsync(new Band())
                .Verifiable();

            var result = await _controler.CreateBand(new CreateBandDTO());
            _mockService.Verify(repo => repo.CreateBand(It.IsAny<CreateBandDTO>()), Times.AtLeastOnce);
        }

        [Fact]
        public async void CreateBand_WithValidDTO_GetOkResponse()
        {
            var bandDto = new CreateBandDTO()
            {
                Name = "Rantes",
                Genre = "Rock Alternativo"
            };
            _mockService.Setup(repo => repo.CreateBand(bandDto))
                .ReturnsAsync(new Band())
                .Verifiable();

            var result = await _controler.CreateBand(bandDto) as ObjectResult;
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void CreateBand_WithInvalidDTO_GetBadRequest()
        {
            var bandDto = new CreateBandDTO()
            {
                Name = "",
                Genre = "Gothic"
            };
            _mockService.Setup(repo => repo.CreateBand(bandDto))
                .Throws(new ArgumentException());

            var result = await _controler.CreateBand(bandDto) as ObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void CreateBand_ErrorWhileCreating_GetServerError()
        {
            var bandDto = new CreateBandDTO()
            {
                Name = "",
                Genre = "Gothic"
            };
            _mockService.Setup(repo => repo.CreateBand(bandDto))
                .Throws(new Exception());

            var result = await _controler.CreateBand(bandDto) as StatusCodeResult;
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async void DeleteBand_CallServiceMethod_AtLeastOnce()
        {
            _mockService.Setup(service => service.DeleteBand(It.IsAny<string>()))
                .Verifiable();

            var result = await _controler.DeleteBand(It.IsAny<string>());
            _mockService.Verify(repo => repo.DeleteBand(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public async void DeleteBand_WithValidSortId_GetOkResponse()
        {
            _mockService.Setup(repo => repo.DeleteBand(It.IsAny<string>()))
                .ReturnsAsync(new Band())
                .Verifiable();

            var result = await _controler.DeleteBand(It.IsAny<string>()) as NoContentResult;
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void DeleteBand_WithInvalidSortId_GetBadRequest()
        {
            _mockService.Setup(repo => repo.DeleteBand(It.IsAny<string>()))
                .Throws(new NullReferenceException());

            var result = await _controler.DeleteBand(It.IsAny<string>()) as ObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void DeleteBand_ErrorWhileDeleting_GetServerError()
        {
            _mockService.Setup(repo => repo.DeleteBand(It.IsAny<string>()))
                .Throws(new Exception());

            var result = await _controler.DeleteBand(It.IsAny<string>()) as StatusCodeResult;
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async void UpdateBand_CallServiceMethod_AtLeastOnce()
        {
            _mockService.Setup(service => service.UpdateBand(It.IsAny<Band>()))
                .Verifiable();

            var result = await _controler.UpdateBand(It.IsAny<Band>());
            _mockService.Verify(repo => repo.UpdateBand(It.IsAny<Band>()), Times.AtLeastOnce);
        }

        [Fact]
        public async void UpdateBand_WithValidBandData_GetOkResponse()
        {
            _mockService.Setup(repo => repo.UpdateBand(It.IsAny<Band>()))
                .ReturnsAsync(new Band())
                .Verifiable();

            var result = await _controler.UpdateBand(It.IsAny<Band>()) as ObjectResult;
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void UpdateBand_WithInvalidBandData_GetBadRequest()
        {
            _mockService.Setup(repo => repo.UpdateBand(It.IsAny<Band>()))
                .Throws(new NullReferenceException());

            var result = await _controler.UpdateBand(It.IsAny<Band>()) as ObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void UpdateBand_ErrorWhileUpdating_GetServerError()
        {
            _mockService.Setup(repo => repo.UpdateBand(It.IsAny<Band>()))
                .Throws(new Exception());

            var result = await _controler.UpdateBand(It.IsAny<Band>()) as StatusCodeResult;
            Assert.Equal(500, result.StatusCode);
        }
    }
}
