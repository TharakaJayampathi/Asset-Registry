
using Microsoft.EntityFrameworkCore;

namespace AssetRegistry.Data
{
    public class JsonDbContext : DbContext
    {
        public JsonDbContext(DbContextOptions<JsonDbContext> options) : base(options) { }

        //public DbSet<Json> JsonData { get; set; }
        //public DbSet<Company> Company { get; set; }
        //public DbSet<Division> Division { get; set; }
        //public DbSet<Location> Location { get; set; }
        //public DbSet<User> User { get; set; }
        //public DbSet<Role> Role { get; set; }
    }
}