namespace Data.Cosmos.Interface
{
    public interface IRepositorioBase<T> where T : class
    {
        Task<List<T>> GetAll(string Query);
        Task<T> Get(string Key);
        Task<T> Save(T obj, string Key);
        Task<bool> Excluir(string Key); 
    }
}
