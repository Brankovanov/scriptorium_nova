using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Scriptorium_Nova.Web.Common;

namespace Scriptorium_Nova.Web.Data.Models;

public class Entry
{
    public Entry()
    {
        Notes = new HashSet<Note>();
        CreatedOn = DateTime.UtcNow;
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(
        EntityValidation.Entry.NameMaxLength,
        MinimumLength = EntityValidation.Entry.NameMinLength,
        ErrorMessage = "Name must be between {2} and {1} characters.")]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(
        EntityValidation.Entry.SummaryMaxLength,
        MinimumLength = EntityValidation.Entry.SummaryMinLength,
        ErrorMessage = "Summary must be between {2} and {1} characters.")]
    public string Summary { get; set; } = null!;

    [StringLength(
        EntityValidation.Entry.DetailsMaxLength,
        ErrorMessage = "Details cannot exceed {1} characters.")]
    public string? Details { get; set; }

    public DateTime CreatedOn { get; set; }

    [ForeignKey(nameof(EntryType))]
    public int EntryTypeId { get; set; }

    public virtual EntryType EntryType { get; set; } = null!;

    public virtual ICollection<Note> Notes { get; set; }
}