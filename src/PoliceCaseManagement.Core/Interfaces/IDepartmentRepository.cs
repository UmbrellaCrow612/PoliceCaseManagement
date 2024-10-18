namespace PoliceCaseManagement.Core.Interfaces
{
    public interface IDepartmentRepository<T, TId> : IGenericRepository<T, TId> where T : class
    {
    }
}
