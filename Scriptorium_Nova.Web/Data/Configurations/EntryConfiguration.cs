using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scriptorium_Nova.Web.Data.Models;

namespace Scriptorium_Nova.Web.Data.Configurations;

public class EntryConfiguration : IEntityTypeConfiguration<Entry>
{
    public void Configure(EntityTypeBuilder<Entry> builder)
    {
        builder
            .HasOne(e => e.EntryType)
            .WithMany(t => t.Entries)
            .HasForeignKey(e => e.EntryTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(e => new { e.EntryTypeId, e.Name })
            .IsUnique();
    }
}