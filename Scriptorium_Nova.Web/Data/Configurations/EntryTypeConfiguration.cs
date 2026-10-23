using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scriptorium_Nova.Web.Data.Models;

public class EntryTypeConfiguration : IEntityTypeConfiguration<EntryType>
{
    public void Configure(EntityTypeBuilder<EntryType> builder)
    {
        builder.HasData(
            new EntryType { Id = 1, Name = "Faction" },
            new EntryType { Id = 2, Name = "Location" },
            new EntryType { Id = 3, Name = "Character" },
            new EntryType { Id = 4, Name = "Artifact" },
            new EntryType { Id = 5, Name = "Event" });
    }
}