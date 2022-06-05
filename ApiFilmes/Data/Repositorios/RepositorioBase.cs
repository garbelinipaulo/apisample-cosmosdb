using Data.Cosmos.Interface;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Cosmos.Repositorios
{
    public class RepositorioBase<T> : IRepositorioBase<T> where T : class
    {

        #region connectionstring

#if Release
        const string xConn = "connRemota";
#else
        const string xConn = "localhost:6379";
#endif

        #endregion


        private Container _container;

        public RepositorioBase(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }
          

        public async Task<bool> Excluir(string Key)
        { 
            var _retorno = await this._container.DeleteItemAsync<T>(Key, new PartitionKey(Key));
            return true;
        }

        public async Task<List<T>> GetAll(string Query)
        { 
            var query = this._container.GetItemQueryIterator<T>(new QueryDefinition(Query));
            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync(); 
                results.AddRange(response.ToList());
            }

            return results; 
        }

        public async Task<T> Get(string Key)
        { 
            try
            {
                ItemResponse<T> response = await this._container.ReadItemAsync<T>(Key, new PartitionKey(Key));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            } 
        }

        public async Task<T> Save(T obj, string Key)
        {
            T _retorno = obj;
            await this._container.CreateItemAsync<T>(obj, new PartitionKey(Key));

            return await Task.FromResult(_retorno);
        }
         
    } 
}
