using Microsoft.EntityFrameworkCore;
using api.models;

namespace api.Data
{
    public class AppDbContext: DbContext
    {
        public DbSet<User> Users => Set<User>();
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
