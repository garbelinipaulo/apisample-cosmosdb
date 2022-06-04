using Data.Redis.Repositorios;

namespace Data.Redis.Services
{
    public class FilmesServices
    {
        public FilmesServices()
        {
            repositorioFilmes = new RepositorioFilmes();
        }

        public RepositorioFilmes repositorioFilmes { get; set; }

    }
}
