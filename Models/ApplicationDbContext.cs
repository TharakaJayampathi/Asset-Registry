using AssetRegistry.Models.Logs;
using AssetRegistry.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly IHttpContextAccessor? _httpContextAccessor;

    // IHttpContextAccessor is optional to allow design-time migrations
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor? httpContextAccessor = null)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Optional: Add any custom configurations
        // builder.Entity<ApplicationUser>().Property(u => u.SomeProperty).HasMaxLength(100);
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<DataTransactionLog> DataTransactionLogs { get; set; }
    public DbSet<TransactionLogDetail> TransactionLogDetails { get; set; }
}
