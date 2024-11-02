namespace PoliceCaseManagement.Infrastructure.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(string id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T updatedEntity);
        Task DeleteAsync(string id);
        /// <summary>
        /// Check if an entity exists by it's ID
        /// </summary>
        /// <param name="id">The ID of the entity</param>
        /// <returns> <see langword="true" /> if it exists or <see langword="false"/> if it dose not.</returns>
        Task<bool> ExistsAsync(string id);
    }
}
