namespace PoliceCaseManagement.Core.Interfaces
{
    public interface ILocationRepository<T, TId> : IGenericRepository<T, TId> where T : class
    {
    }
}
