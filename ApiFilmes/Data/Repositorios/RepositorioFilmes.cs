using Data.Cosmos.Interface;
using Domain.Models;
using Microsoft.Azure.Cosmos;

namespace Data.Cosmos.Repositorios
{
    public class RepositorioFilmes : RepositorioBase<FilmesModel>, IRepositorioFilmes
    { 
        private Container _container; 
        public RepositorioFilmes(
            CosmosClient dbClient,
            string databaseName, 
            string containerName) : base(dbClient, databaseName, containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }
    }
}
