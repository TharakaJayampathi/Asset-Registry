using AssetRegistry.Models.Logs;
using AssetRegistry.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
        ChangeTracker.DetectChanges();

        var modifiedEntities = ChangeTracker.Entries();
        var logList = new List<DataTransactionLog>();

        foreach (var change in modifiedEntities)
        {
            var state = change.State;
            if (state == EntityState.Added || state == EntityState.Modified || state == EntityState.Deleted)
            {
                var entityName = change.Entity.GetType().Name;
                var currentValues = change.CurrentValues;
                var originalValues = change.OriginalValues;

                var log = new DataTransactionLog
                {
                    EntityName = entityName,
                    UserName = _httpContextAccessor?.HttpContext?.User?.Identity?.Name,
                    TransactionType = state.ToString(),
                    TransactionDate = DateTime.Now
                };

                var logDetails = new List<TransactionLogDetail>();
                foreach (var property in currentValues.Properties)
                {
                    logDetails.Add(new TransactionLogDetail
                    {
                        PropertyName = property.Name,
                        CurrentValue = currentValues[property.Name]?.ToString(),
                        OriginalValue = originalValues[property.Name]?.ToString()
                    });
                }

                log.LogDetails = JsonConvert.SerializeObject(logDetails);
                logList.Add(log);
            }
        }

        if (logList.Any())
            await base.AddRangeAsync(logList, cancellationToken);

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
