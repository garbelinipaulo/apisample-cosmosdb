using Data.Redis.Interfaces;
using Domain.Models;
using ServiceStack.Redis;

namespace Data.Redis.Repositorios
{
    public class RepositorioFilmes : RepositorioBase<FilmesModel>, IRepositorioFilmes
    {

#if Release
        const string xConn = "connRemota";
#else
        const string xConn = "172.17.0.2:6379"; //ip local do docker
#endif

        public void SaveLogFilmesBuscados(string xMessage, string Key)
        {
            using (var _redisClient = new RedisClient(xConn))
            {
                _redisClient.Set(Key, xMessage);
            }
        }

        public async Task<List<string>> BuscaLogsGravados()
        {
            List<string> list = new List<string>();
            using (var _redisClient = new RedisClient(xConn))
            {
                var _listaKeys = _redisClient.GetAllKeys();
                foreach (var item in _listaKeys.Where(t => t.StartsWith("Logger")))
                {
                    list.Add(_redisClient.Get<string>(item));
                }
            }

            return await Task.FromResult(list); 
        }

    }
}
