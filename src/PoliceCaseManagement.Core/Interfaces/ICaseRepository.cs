namespace PoliceCaseManagement.Core.Interfaces
{
    public interface ICaseRepository<T, TId> : IGenericRepository<T, TId> where T : class
    {
    }
}
