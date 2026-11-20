using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Scriptorium_Nova.Web.Data.Models;

namespace Scriptorium_Nova.Web.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<EntryType> EntryTypes { get; set; } = null!;
    public DbSet<Entry> Entries { get; set; } = null!;
    public DbSet<Note> Notes { get; set; } = null!;

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplyAuditRules();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        ApplyAuditRules();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(builder);
    }

    private void ApplyAuditRules()
    {
        DateTime now = DateTime.UtcNow;

        foreach (EntityEntry<AuditableEntity> entry in ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State != EntityState.Modified)
            {
                continue;
            }

            entry.Property(e => e.CreatedOn).IsModified = false;
            entry.Property(e => e.ModifiedOn).CurrentValue = now;
        }
    }
}