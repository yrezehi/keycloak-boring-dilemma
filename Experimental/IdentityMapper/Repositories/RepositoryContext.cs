using Microsoft.EntityFrameworkCore;

namespace IdentityMapper.Repositories
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options) : base(options) { }
    }
}
