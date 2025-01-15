using Identity.Core.Repositorys;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data.Repositorys
{
    internal class Repository<T>(IdentityApplicationDbContext context) : IRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet = context.Set<T>();

        public IQueryable<T> Query => _dbSet.AsQueryable();

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<T?> FindByIdAsync(string id) => await _dbSet.FindAsync(id);

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);
    }
}
