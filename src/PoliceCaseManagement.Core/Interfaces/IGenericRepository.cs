namespace PoliceCaseManagement.Core.Interfaces
{
    /// <summary>
    /// Basic Generic Repository for entities that need little custom implementations.
    /// </summary>
    /// <remarks>
    /// Entities that need extra functionality should extend this base interface.
    /// </remarks>
    public interface IGenericRepository<TEntity, TId> where TEntity : class
    {
        /// <summary>
        /// Fetch an entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>The entity or null if it could not be found.</returns>
        Task<TEntity?> GetByIdAsync(TId id);

        /// <summary>
        /// Check if an entity exists by it's ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns><see langword="true"/> if it does <see langword="false"/> id it does not.</returns>
        Task<bool> ExistsAsync(TId id);

        /// <summary>
        /// Add an entity to the database.
        /// </summary>
        /// <param name="entity">The entity you want to add.</param>
        /// <remarks>It is assumed you have validated the shape of the entity before adding it.</remarks>
        Task AddAsync(TEntity entity);

        /// <summary>
        /// Update an entity
        /// </summary>
        /// <param name="entity">The entity with updated fields.</param>
        /// <remarks>It is assumed you have validated the shape of the entity before updating it and it exists, pass the updated entity to this.</remarks>
        Task UpdateAsync(TEntity updatedEntity);

        /// <summary>
        /// Delete an entity
        /// </summary>
        /// <param name="id">The id of the entity to delete.</param>
        /// <returns><see langword="true"/> if successful or <see langword="false"/> if it could not be found.</returns>
        Task<bool> DeleteAsync(TId id);
    }
}