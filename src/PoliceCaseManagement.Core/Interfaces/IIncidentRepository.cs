namespace PoliceCaseManagement.Core.Interfaces
{
    public interface IIncidentRepository<T, TId> : IGenericRepository<T, TId> where T : class
    {
    }
}
