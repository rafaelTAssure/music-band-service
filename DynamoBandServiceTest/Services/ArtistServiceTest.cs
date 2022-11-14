using Amazon.DynamoDBv2.DocumentModel;
using DynamoBandService.Models;
using DynamoBandService.Models.DTOs;
using DynamoBandService.Repositories.Interfaces;
using DynamoBandService.Services;
using Moq;

namespace DynamoBandServiceTest.Services
{
    public class ArtistServiceTest
    {
        private readonly Mock<IRepository<Artist>> _repository;
        private readonly ArtistService _service;
        public ArtistServiceTest()
        {
            _repository = new Mock<IRepository<Artist>>();
            _service = new ArtistService(_repository.Object);
        }

        [Fact]
        public async void CreateArtist_CallRepositoryMethod_AtLeastOnce()
        {
            _repository.Setup(repo => repo.Save(It.IsAny<Artist>()))
                .Verifiable();

            var validArtistDto = new CreateArtistDTO()
            {
                BandSortId = "Band#4830aa33-3e70-4c02-8118-99a0feaeec3d",
                Name = "ValidBandName",
                Email = "test@gmail.com"
            };

            var result = await _service.CreateArtist(validArtistDto);

            _repository.Verify(repo => repo.Save(It.IsAny<Artist>()), Times.AtLeastOnce);
        }

        [Fact]
        public async void CreateArtist_InvalidBandSortID_GetArgumentException()
        {
            var validArtistDto = new CreateArtistDTO()
            {
                BandSortId = "",
                Name = "ValidBandName",
                Email = "test@gmail.com"
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateArtist(validArtistDto));
        }

        [Fact]
        public async void DeleteArtist_CallRepositoryMethod_AtLeastOnce()
        {
            _repository.Setup(repo => repo.Delete(It.IsAny<Artist>()))
                .Verifiable();

            _repository.Setup(repo => repo.Load(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Artist() { Id = "ARTIST", SortId = "BAND#7156fdb6-1d2d-41f4-af32-c4173bb1f336#7db55e90-60b9-412f-9084-7294ef20a111" });

            var result = await _service.DeleteArtist(It.IsAny<string>());

            _repository.Verify(repo => repo.Delete(It.IsAny<Artist>()), Times.AtLeastOnce);
        }

        [Fact]
        public async void DeleteArtist_InvalidSortID_GetNullReferenceException()
        {
            _repository.Setup(repo => repo.Load(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((Artist)null);

            await Assert.ThrowsAsync<NullReferenceException>(() => _service.DeleteArtist(It.IsAny<string>()));
        }

        [Fact]
        public async void GetAtistByBand_CallRepositoryMethod_AtLeastOnce()
        {
            _repository.Setup(repo => repo.Query(It.IsAny<string>(), It.IsAny<QueryOperator>(), It.IsAny<List<object>>()))
                .Verifiable();

            var result = await _service.GetArtistByBand(It.IsAny<string>());

            _repository.Verify(repo => repo.Query(It.IsAny<string>(), It.IsAny<QueryOperator>(), It.IsAny<List<object>>()), Times.AtLeastOnce);
        }

        [Fact]
        public async void GetAtistById_CallRepositoryMethod_AtLeastOnce()
        {
            _repository.Setup(repo => repo.Load(It.IsAny<string>(), It.IsAny<string>()))
                .Verifiable();

            var result = await _service.GetArtistById(It.IsAny<string>());

            _repository.Verify(repo => repo.Load(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public async void UpdateArtist_CallRepositoryMethod_AtLeastOnce()
        {
            _repository.Setup(repo => repo.Save(It.IsAny<Artist>()))
                .Verifiable();

            _repository.Setup(repo => repo.Load(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Artist() { Id = "ARTIST", SortId = "BAND#7156fdb6-1d2d-41f4-af32-c4173bb1f336#7db55e90-60b9-412f-9084-7294ef20a111", Name = "validArtist" });

            var validArtistDto = new Artist()
            {
                Id = "BAND",
                SortId = "BAND#7156fdb6-1d2d-41f4-af32-c4173bb1f336#7db55e90-60b9-412f-9084-7294ef20a111",
                Name = "validArtist"
            };

            var result = await _service.UpdateArtist(validArtistDto);

            _repository.Verify(repo => repo.Save(It.IsAny<Artist>()), Times.AtLeastOnce);
        }

        [Fact]
        public async void UpdateArtist_EditArtistAttributes_GetArtistWithNewValues()
        {
            _repository.Setup(repo => repo.Save(It.IsAny<Artist>()))
                .Verifiable();

            _repository.Setup(repo => repo.Load(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Artist() { Id = "BAND", SortId = "BAND#7156fdb6-1d2d-41f4-af32-c4173bb1f336#7db55e90-60b9-412f-9084-7294ef20a111", Name = "validArtist" });

            var validArtistDto = new Artist()
            {
                Id = "ARTIST",
                SortId = "BAND#7156fdb6-1d2d-41f4-af32-c4173bb1f336#7db55e90-60b9-412f-9084-7294ef20a111",
                Name = "validArtistEditedName"
            };

            var result = await _service.UpdateArtist(validArtistDto);

            Assert.Equal("validArtistEditedName", result.Name);
        }

        [Fact]
        public async Task UpdateArtist_ReceiveInvalidData_GetArgumentNullException()
        {
            var validBandDto = new Artist()
            {
                Id = "",
                SortId = "",
                Name = "Jhon Doe",
            };

            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.UpdateArtist(validBandDto));
        }
    }
}
