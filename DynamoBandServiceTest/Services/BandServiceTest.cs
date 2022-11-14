using Amazon.DynamoDBv2.DocumentModel;
using DynamoBandService.Models;
using DynamoBandService.Models.DTOs;
using DynamoBandService.Repositories.Interfaces;
using DynamoBandService.Services;
using Moq;

namespace DynamoBandServiceTest.Services
{
    public class BandServiceTest
    {
        private readonly Mock<IRepository<Band>> _repository;
        private readonly BandService _service;
        public BandServiceTest()
        {
            _repository = new Mock<IRepository<Band>>();
            _service = new BandService(_repository.Object);
        }

        [Fact]
        public async void GetBandById_CallRepositoryMethod_AtLeastOnce()
        {
            _repository.Setup(repo => repo.Load(It.IsAny<string>(), It.IsAny<string>()))
                .Verifiable();

            var result = await _service.GetBandById(It.IsAny<string>());

            _repository.Verify(repo => repo.Load(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public async void GetAllBands_CallRepositoryMethod_AtLeastOnce()
        {
            _repository.Setup(repo => repo.Query(It.IsAny<string>(), It.IsAny<QueryOperator>(), It.IsAny<List<object>>()))
                .Verifiable();

            var result = await _service.GetAllBands();

            _repository.Verify(repo => repo.Query(It.IsAny<string>(), It.IsAny<QueryOperator>(), It.IsAny<List<object>>()), Times.AtLeastOnce);
        }

        [Fact]
        public async void CreateBand_CallRepositoryMethod_AtLeastOnce()
        {
            _repository.Setup(repo => repo.Save(It.IsAny<Band>()))
                .Verifiable();

            var validBandDto = new CreateBandDTO()
            {
                Name = "ValidBandName",
                Genre = "Rock"
            };

            var result = await _service.CreateBand(validBandDto);

            _repository.Verify(repo => repo.Save(It.IsAny<Band>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task CreateBand_ReceivingAnInvalidBandDTO_GetArgumentException()
        {
            _repository.Setup(repo => repo.Save(It.IsAny<Band>()))
                .Verifiable();

            var invalidBandDto = new CreateBandDTO()
            {
                Name = "",
                Genre = "Rock"
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateBand(invalidBandDto));
        }

        [Fact]
        public async Task CreateBand_ReceiveValidData_GetABandInstance()
        {
            _repository.Setup(repo => repo.Save(It.IsAny<Band>()))
                .Verifiable();

            var validBandDto = new CreateBandDTO()
            {
                Name = "Sum 41",
                Genre = "Rock"
            };

            var band = await _service.CreateBand(validBandDto);

            Assert.Equal(validBandDto.Name, band.Name);
        }

        [Fact]
        public async void DeleteBand_CallRepositoryMethod_AtLeastOnce()
        {
            _repository.Setup(repo => repo.Delete(It.IsAny<Band>()))
                .Verifiable();

            _repository.Setup(repo => repo.Load(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Band() { Id = "BAND", SortId = "Band#4830aa33-3e70-4c02-8118-99a0feaeec3d", Name = "bandName"});

            var result = await _service.DeleteBand(It.IsAny<string>());

            _repository.Verify(repo => repo.Delete(It.IsAny<Band>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task DeleteBand_ReceivingAnInvalidSortID_GetNullReferenceException()
        {
            _repository.Setup(repo => repo.Load(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((Band)null);

            await Assert.ThrowsAsync<NullReferenceException>(() => _service.DeleteBand(It.IsAny<string>()));
        }

        [Fact]
        public async void UpdateBand_CallRepositoryMethod_AtLeastOnce()
        {
            _repository.Setup(repo => repo.Save(It.IsAny<Band>()))
                .Verifiable();

            _repository.Setup(repo => repo.Load(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Band() { Id="BAND", SortId= "Band#4830aa33-3e70-4c02-8118-99a0feaeec3d", Name="validBand" })
                .Verifiable();

            var validBandDto = new Band() 
            { 
                Id = "BAND", 
                SortId = "Band#4830aa33-3e70-4c02-8118-99a0feaeec3d", 
                Name = "validBand" 
            };

            var result = await _service.UpdateBand(validBandDto);

            _repository.Verify(repo => repo.Save(It.IsAny<Band>()), Times.AtLeastOnce);
        }

        [Fact]
        public async void UpdateBand_EditBandAttributes_GetBandWithNewValues()
        {
            _repository.Setup(repo => repo.Save(It.IsAny<Band>()))
                .Verifiable();

            _repository.Setup(repo => repo.Load(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Band() { Id = "BAND", SortId = "Band#4830aa33-3e70-4c02-8118-99a0feaeec3d", Name = "validBand" })
                .Verifiable();

            var validBandDto = new Band()
            {
                Id = "BAND",
                SortId = "Band#4830aa33-3e70-4c02-8118-99a0feaeec3d",
                Name = "validBandEditedName"
            };

            var result = await _service.UpdateBand(validBandDto);

            Assert.Equal("validBandEditedName", result.Name);
        }

        [Fact]
        public async Task UpdateBand_ReceiveInvalidData_GetNullReferenceException()
        {
            var validBandDto = new Band()
            {
                Id = "",
                SortId = "",
                Name = "Sum 41",
                Genre = "Rock"
            };

            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.UpdateBand(validBandDto));
        }
    }
}
