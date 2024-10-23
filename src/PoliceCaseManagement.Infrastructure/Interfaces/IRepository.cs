namespace PoliceCaseManagement.Infrastructure.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(string id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T updatedEntity);
        Task DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
    }
}
