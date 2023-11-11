using Callem.Models.DTO;
using Callem.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace IdentityMapper.Repositories.Extentions
{
    public static class LinqExtensions
    {

        public static IQueryable<IEntity> WhereIf<IEntity>(this DbSet<IEntity> source, bool condition, Expression<Func<IEntity, bool>> predicate) where IEntity : class
        {
            return condition ? source.Where(predicate) : source;
        }

        public static async Task<int> ItemsCount<IEntity>(this DbSet<IEntity> source, Expression<Func<IEntity, bool>> predicate = null) where IEntity : class
        {
            return predicate != null ? await source.Where(predicate).AsQueryable().CountAsync() : await source.CountAsync();
        }

    }
}
