namespace PoliceCaseManagement.Core.Interfaces
{
    public interface ICaseRepository<T, TId> : IGenericRepository<T, TId> where T : class
    {
        /// <summary>
        /// Search for cases based on specified criteria.
        /// </summary>
        /// <returns>A collection of cases that match the search criteria.</returns>
        Task<IEnumerable<T>> SearchAsync();
    }
}
