using Data.Redis.Interfaces;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Redis.Repositorios
{
    public class RepositorioBase<T> : IRepositorio<T> where T : class
    {
        #region connectionstring

#if Release
        const string xConn = "connRemota";
#else
        const string xConn = "localhost:6379";
#endif

        #endregion


        public async Task<bool> Excluir(string Key)
        {
            bool _retorno = false;
            using (var _redisClient = new RedisClient(xConn))
            {
                try
                { 
                    _retorno = _redisClient.Remove(Key);
                }
                catch (Exception ex)
                {
                    _retorno = false;
                }
            }

            return await Task.FromResult(_retorno);
        }

        public async Task<List<T>> GetAll()
        { 
            List<T> list = new List<T>();
            using (var _redisClient = new RedisClient(xConn))
            { 
                var _retorno = _redisClient.GetAll<T>();
            }

            return await Task.FromResult(list);
        }

        public async Task<T> Get(string Key)
        {
            T _retorno;
            using (var _redisClient = new RedisClient(xConn))
            { 
                _retorno = _redisClient.Get<T>(Key);
            }

            return await Task.FromResult(_retorno);
        }

        public async Task<T> Save(T obj, string Key)
        {
            T _retorno = obj;
            using (var _redisClient = new RedisClient(xConn))
            { 
                _redisClient.Set<T>(Key, obj);
                _retorno = _redisClient.Get<T>(Key);    
            }

            return await Task.FromResult(_retorno);
        }

        public void SaveCollection(Dictionary<string,T> objs)
        { 
            using (var _redisClient = new RedisClient(xConn))
            {
                _redisClient.SetAll<T>(objs); 
            } 
        }
    }
}
