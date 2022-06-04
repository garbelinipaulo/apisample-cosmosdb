namespace Data.Redis.Interfaces
{
    public interface IRepositorio<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> Get(string Key);
        Task<T> Save(T obj, string Key); 
        Task<bool> Excluir(string Key);
        void SaveCollection(Dictionary<string, T> objs);
    }
}
