namespace Identity.Core.Repositorys
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
        Task SaveChangesAsync();
    }
}
