using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using DynamoBandService.Repositories.Interfaces;

namespace DynamoBandService.Repositories
{
    public class Repository<T> : IRepository<T>
    {
        private readonly IDynamoDBContext _context;
        public Repository(IDynamoDBContext context)
        {
            _context = context;
        }

        public async Task Delete(T artist)
        {
            await _context.DeleteAsync(artist);
        }

        public async Task<T> Load(string partitionKey, string sortId)
        {
            var element = await _context.LoadAsync<T>(partitionKey, sortId);
            return element;
        }

        public async Task<List<T>> Query(string partitionKey, QueryOperator operation, List<object> queryValues)
        {
            var elements = await _context.QueryAsync<T>(partitionKey, operation, queryValues).GetRemainingAsync();
            return elements;
        }

        public async Task Save(T artist)
        {
            await _context.SaveAsync(artist);
        }
    }
}
