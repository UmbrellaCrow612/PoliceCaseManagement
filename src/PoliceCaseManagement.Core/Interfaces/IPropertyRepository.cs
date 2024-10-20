namespace PoliceCaseManagement.Core.Interfaces
{
    public interface IPropertyRepository<T, TId> : IGenericRepository<T, TId> where T : class
    {
    }
}
