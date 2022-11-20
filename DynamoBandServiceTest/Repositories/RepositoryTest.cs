using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using DynamoBandService.Models;
using DynamoBandService.Repositories;
using Moq;

namespace DynamoBandServiceTest.Repositories
{
    public class RepositoryTest
    {
        private readonly Mock<IDynamoDBContext> _mockContext;
        private readonly Repository<Band> _repository; 
        public RepositoryTest()
        {
            _mockContext = new Mock<IDynamoDBContext>();
            _repository = new Repository<Band>(_mockContext.Object);
        }

        [Fact]
        public async void Load_CallContextMethod_AtLeastOnce()
        {
            _mockContext.Setup(con => con.LoadAsync<Band>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));

            await _repository.Load(It.IsAny<string>(), It.IsAny<string>());

            _mockContext.Verify(con => con.LoadAsync<Band>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        [Fact]
        public async void Delete_CallContextMethod_AtLeastOnce()
        {
            _mockContext.Setup(con => con.DeleteAsync(It.IsAny<Band>(), It.IsAny<CancellationToken>()));

            await _repository.Delete(It.IsAny<Band>());

            _mockContext.Verify(con => con.DeleteAsync<Band>(It.IsAny<Band>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        [Fact]
        public async void Query_CallContextMethod_AtLeastOnce()
        {
            List<object> queryVal = new()
            {
                "BAND#"
            };

            _mockContext.Setup(con => con.QueryAsync<Band>("", QueryOperator.BeginsWith, queryVal, It.IsAny<DynamoDBOperationConfig>()).GetRemainingAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<List<Band>>())
                .Verifiable();

            await _repository.Query("", QueryOperator.BeginsWith, queryVal);

            _mockContext.Verify(con => con.QueryAsync<Band>(It.IsAny<string>(), It.IsAny<QueryOperator>(), It.IsAny<List<object>>(), It.IsAny<DynamoDBOperationConfig>()), Times.AtLeastOnce);
        }

        [Fact]
        public async void Save_CallContextMethod_AtLeastOnce()
        {
            _mockContext.Setup(con => con.SaveAsync(It.IsAny<Band>(), It.IsAny<CancellationToken>()));

            await _repository.Save(It.IsAny<Band>());

            _mockContext.Verify(con => con.SaveAsync<Band>(It.IsAny<Band>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }
    }
}
