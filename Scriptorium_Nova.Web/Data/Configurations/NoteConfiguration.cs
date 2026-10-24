using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scriptorium_Nova.Web.Data.Models;

namespace Scriptorium_Nova.Web.Data.Configurations;

public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder
            .HasOne(n => n.Entry)
            .WithMany(e => e.Notes)
            .HasForeignKey(n => n.EntryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}