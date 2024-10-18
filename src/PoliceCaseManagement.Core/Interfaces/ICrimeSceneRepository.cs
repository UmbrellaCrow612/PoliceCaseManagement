namespace PoliceCaseManagement.Core.Interfaces
{
    public interface ICrimeSceneRepository<T, TId> : IGenericRepository<T, TId> where T : class
    {
    }
}
