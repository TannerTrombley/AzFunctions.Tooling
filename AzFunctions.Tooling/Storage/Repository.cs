using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AzFunctions.Tooling.Storage
{
    public class Repository<T> : IRepository<T> where T : DocumentBase
    {
        private CloudTableClient _client;
        private CloudStorageAccount _storageAccount;
        private CloudTable _table;

        public Repository(string connectionString, string tableName)
        {
            _storageAccount = CreateStorageAccountFromConnectionString(connectionString);
            _client = _storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            _table = _client.GetTableReference(tableName);
        }

        public Task<T> CreateDocumentAsync(T document)
        {
            var operation = TableOperation.Insert(document);
            return ExecuteInternalAsync(operation, "Create");
        }

        public Task<T> UpsertDocumentAsync(T document)
        {
            var operation = TableOperation.InsertOrMerge(document);
            return ExecuteInternalAsync(operation, "Upsert");
        }

        public async Task<T> DeleteDocumentAsync(string id, string partitionKey)
        {
            var doc = await GetDocumentAsync(id, partitionKey);
            var operation = TableOperation.Delete(doc);
            return await ExecuteInternalAsync(operation, "Delete");
        }

        public Task<T> GetDocumentAsync(string id, string partitionKey)
        {
            var operation = TableOperation.Retrieve(partitionKey, id);
            return ExecuteInternalAsync(operation, "Get");
        }

        private async Task<T> ExecuteInternalAsync(TableOperation operation, string operationName)
        {
            var result = await _table.ExecuteAsync(operation);
            if (result.HttpStatusCode < 300)
            {
                return result.Result as T;
            }
            else
            {
                throw new RepositoryException($"Faield to {operationName} Document");
            }
        }


        private static CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the application.");
                throw;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                Console.ReadLine();
                throw;
            }

            return storageAccount;
        }
    }
}
