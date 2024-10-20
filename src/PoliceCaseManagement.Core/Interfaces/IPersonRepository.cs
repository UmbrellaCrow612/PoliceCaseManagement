namespace PoliceCaseManagement.Core.Interfaces
{
    public interface IPersonRepository<T, TId> : IGenericRepository<T, TId> where T : class
    {
    }
}
