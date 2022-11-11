using Amazon.DynamoDBv2.DocumentModel;
using DynamoBandService.Models;

namespace DynamoBandService.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        Task Save(T artist);
        Task<T> Load(string partitionKey, string sortId);
        Task Delete(T artist);
        Task<List<T>> Query(string partitionKey, QueryOperator operation, List<object> queryValues);
    }
}
