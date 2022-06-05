using Data.Redis.Interfaces;
using Data.Redis.Repositorios;

namespace Data.Redis.Services
{
    public class FilmesServices
    {
        public FilmesServices()
        {
            repositorioFilmes = new RepositorioFilmes();
        }

        public IRepositorioFilmes repositorioFilmes { get; set; }

    }
}
