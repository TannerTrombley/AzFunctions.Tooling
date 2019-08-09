using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AzFunctions.Tooling.Storage
{
    public interface IRepository<T>
    {
        Task<T> CreateDocumentAsync(T document);

        Task<T> GetDocumentAsync(string id, string partitionKey);

        Task<T> DeleteDocumentAsync(string id, string partitionKey);

        Task<T> UpsertDocumentAsync(T document);
    }
}
