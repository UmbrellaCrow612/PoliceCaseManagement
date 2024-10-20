namespace PoliceCaseManagement.Core.Interfaces
{
    public interface IReportRepository<T, TId> : IGenericRepository<T, TId> where T : class
    {
    }
}
