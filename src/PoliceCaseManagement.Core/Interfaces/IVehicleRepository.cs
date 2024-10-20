namespace PoliceCaseManagement.Core.Interfaces
{
    public interface IVehicleRepository<T, TId> : IGenericRepository<T, TId> where T : class
    {
    }
}
