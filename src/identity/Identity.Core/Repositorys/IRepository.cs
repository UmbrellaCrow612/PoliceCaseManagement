namespace Identity.Core.Repositorys
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task<T?> FindByIdAsync(string id);
        void Update(T entity);
        void UpdateRange(ICollection<T> values);
        void Delete(T entity);
        IQueryable<T> Query { get; }
    }
}
