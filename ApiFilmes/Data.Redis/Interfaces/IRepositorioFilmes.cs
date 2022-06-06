using Domain.Models;

namespace Data.Redis.Interfaces
{
    public interface IRepositorioFilmes : IRepositorio<FilmesModel> 
    {
        void SaveLogFilmesBuscados(string xMessage, string Key);
        Task<List<string>> BuscaLogsGravados();
    }
}
