using Microsoft.EntityFrameworkCore;
using AssetRegistry.Models;

namespace AssetRegistry.Data
{
    public class JsonDbContext : DbContext
    {
        public JsonDbContext(DbContextOptions<JsonDbContext> options) : base(options) { }

        public DbSet<Json> JsonData { get; set; }
    }
}