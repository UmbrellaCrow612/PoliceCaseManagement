namespace Identity.Core.Repositorys
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task<T?> GetByIdAsync(string id);
        void Update(T entity);
        void Delete(T entity);
    }
}
