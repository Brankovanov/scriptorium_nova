using Microsoft.EntityFrameworkCore;
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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(builder);
    }
}