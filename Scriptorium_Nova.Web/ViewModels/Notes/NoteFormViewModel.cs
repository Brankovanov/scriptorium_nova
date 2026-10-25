using System.ComponentModel.DataAnnotations;
using Scriptorium_Nova.Web.Common;
using Scriptorium_Nova.Web.Common.Attributes;

namespace Scriptorium_Nova.Web.ViewModels.Notes;

public class NoteFormViewModel
{
    public int EntryId { get; set; }

    [Required(ErrorMessage = "Please enter a title.")]
    [StringLength(
        EntityValidation.Note.TitleMaxLength,
        MinimumLength = EntityValidation.Note.TitleMinLength,
        ErrorMessage = "Title must be between {2} and {1} characters.")]
    [MustContainLetter]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Please enter the note text.")]
    [StringLength(
        EntityValidation.Note.TextMaxLength,
        MinimumLength = EntityValidation.Note.TextMinLength,
        ErrorMessage = "Text must be between {2} and {1} characters.")]
    public string Text { get; set; } = null!;
}