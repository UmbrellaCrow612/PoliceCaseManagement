using Identity.Core.Repositorys;

namespace Identity.Infrastructure.Data.Repositorys
{
    internal class UnitOfWork(IdentityApplicationDbContext context) : IUnitOfWork
    {
        private readonly IdentityApplicationDbContext _context = context;
        private readonly Dictionary<Type, object> _repositories = [];

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (!_repositories.ContainsKey(typeof(TEntity)))
            {
                _repositories[typeof(TEntity)] = new Repository<TEntity>(_context);
            }
            return (IRepository<TEntity>)_repositories[typeof(TEntity)];
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
