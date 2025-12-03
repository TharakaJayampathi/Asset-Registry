using Microsoft.EntityFrameworkCore;
using test_backend.Models;

namespace test_backend.Data
{
    public class JsonDbContext : DbContext
    {
        public JsonDbContext(DbContextOptions<JsonDbContext> options) : base(options) { }

        public DbSet<Json> JsonData { get; set; }
    }
}