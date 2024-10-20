namespace PoliceCaseManagement.Core.Interfaces
{
    public interface IStatementRepository<T, TId> : IGenericRepository<T, TId> where T : class
    {
    }
}
