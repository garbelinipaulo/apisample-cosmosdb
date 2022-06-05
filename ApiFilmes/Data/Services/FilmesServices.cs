using Data.Cosmos.Interface;
using Data.Cosmos.Repositorios;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Cosmos.Services
{
    public class FilmesServices
    {
        string databaseName;
        string containerName;
        string account;
        string key;
        public FilmesServices()
        {
            repositorioFilmes = new RepositorioFilmes(new Microsoft.Azure.Cosmos.CosmosClient(account, key), databaseName, containerName);
        }

        public IRepositorioFilmes repositorioFilmes { get; set; } 
    }

}
