namespace PoliceCaseManagement.Core.Interfaces
{
    public interface ICrimeSceneRepository<T, TId> : IGenericRepository<T, TId> where T : class
    {
        /// <summary>
        /// Search for crime scenes based on specified criteria.
        /// </summary>
        /// <returns>A collection of crime scenes that match the search criteria.</returns>
        Task<IEnumerable<T>> SearchAsync();
    }
}
