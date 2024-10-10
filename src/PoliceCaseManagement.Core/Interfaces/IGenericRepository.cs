namespace PoliceCaseManagement.Core.Interfaces
{
    /// <summary>
    /// Basic Generic Repository for entities that need little custom implementations.
    /// </summary>
    /// <remarks>
    /// Entities that need extra functionality should extend this base interface.
    /// </remarks>
    public interface IGenericRepository<T, TId> where T : class
    {
        /// <summary>
        /// Fetch an entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity</param>
        /// <returns>The entity or null if it could not be found.</returns>
        Task<T?> GetByIdAsync(TId id);

        /// <summary>
        /// Fetch all entities.
        /// </summary>
        /// <returns>An IEnumerable of all entities.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Add an entity to the database.
        /// </summary>
        /// <param name="entity">The entity you want to add.</param>
        /// <remarks>It is assumed you have validated the shape of the entity before adding it.</remarks>
        Task AddAsync(T entity);

        /// <summary>
        /// Add multiple entities to the database.
        /// </summary>
        /// <param name="entities">The entities you want to add.</param>
        Task AddRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Update an entity
        /// </summary>
        /// <param name="entity">The entity with updated fields.</param>
        /// <returns><see langword="true"/> if successful or <see langword="false"/> if it could not be found.</returns>
        /// <remarks>It is assumed you have validated the shape of the entity before updating it.</remarks>
        Task<bool> UpdateAsync(T entity);

        /// <summary>
        /// Update multiple entities
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        /// <returns>The number of entities successfully updated.</returns>
        Task<int> UpdateRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Delete an entity
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns><see langword="true"/> if successful or <see langword="false"/> if it could not be found.</returns>
        Task<bool> DeleteAsync(T entity);

        /// <summary>
        /// Save changes to the database.
        /// </summary>
        Task SaveChangesAsync();
    }
}