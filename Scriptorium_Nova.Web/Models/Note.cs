using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Scriptorium_Nova.Web.Common;

namespace Scriptorium_Nova.Web.Data.Models;

public class Note
{
    public Note()
    {
        CreatedOn = DateTime.UtcNow;
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(
        EntityValidation.Note.TitleMaxLength,
        MinimumLength = EntityValidation.Note.TitleMinLength,
        ErrorMessage = "Title must be between {2} and {1} characters.")]
    public string Title { get; set; } = null!;

    [Required]
    [StringLength(
        EntityValidation.Note.TextMaxLength,
        MinimumLength = EntityValidation.Note.TextMinLength,
        ErrorMessage = "Text must be between {2} and {1} characters.")]
    public string Text { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    [ForeignKey(nameof(Entry))]
    public int EntryId { get; set; }

    public virtual Entry Entry { get; set; } = null!;
}