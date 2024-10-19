namespace PoliceCaseManagement.Core.Interfaces
{
    public interface IDocumentRepository<T, TId> : IGenericRepository<T, TId> where T : class
    {
    }
}
