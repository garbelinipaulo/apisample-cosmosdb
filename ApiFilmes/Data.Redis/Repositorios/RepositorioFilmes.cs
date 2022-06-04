using Data.Redis.Interfaces;
using Domain.Models;

namespace Data.Redis.Repositorios
{
    public class RepositorioFilmes : RepositorioBase<FilmesModel>, IRepositorioFilmes
    { 
    }
}
