namespace PoliceCaseManagement.Core.Interfaces
{
    public interface IUserRepository<T, TId> : IGenericRepository<T, TId> where T : class
    {
    }
}
