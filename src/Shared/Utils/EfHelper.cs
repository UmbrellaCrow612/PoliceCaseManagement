using Microsoft.EntityFrameworkCore;

namespace Utils
{
    public static class EfHelper
    {
        public static bool ExistsInContext<TContext, TEntity>(this TContext context, TEntity entity)
        where TContext : DbContext
        where TEntity : class
        {
            return context.Set<TEntity>().Local.Any(e => e == entity);
        }
    }
}
