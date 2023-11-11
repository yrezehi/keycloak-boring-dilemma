using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IdentityMapper.Repositories.Interfaces
{
    public interface IRepository<T> : IDisposable where T : class
    {
        public DbSet<T> DBSet { get; }
    }
}
