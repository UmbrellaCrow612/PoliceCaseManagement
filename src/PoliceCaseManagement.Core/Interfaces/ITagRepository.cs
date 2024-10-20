namespace PoliceCaseManagement.Core.Interfaces
{
    public interface ITagRepository<T, TId> : IGenericRepository<T, TId> where T : class
    {
    }
}
