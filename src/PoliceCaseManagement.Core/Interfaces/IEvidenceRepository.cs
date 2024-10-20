namespace PoliceCaseManagement.Core.Interfaces
{
    public interface IEvidenceRepository<T, TId> : IGenericRepository<T, TId> where T : class
    {
    }
}
