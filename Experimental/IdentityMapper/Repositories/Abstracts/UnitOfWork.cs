using IdentityMapper.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityMapper.Repositories.Abstracts
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext>, IUnitOfWork where TContext : DbContext, IDisposable
    {
        public TContext Context { get; }

        public UnitOfWork(TContext context)
        {
            Context = context;
        }

        public async Task<int> CompletedAsync()
        {
            return await Context.SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            await Context.DisposeAsync();
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            return new Repository<TEntity>(Context);
        }

    }
}
