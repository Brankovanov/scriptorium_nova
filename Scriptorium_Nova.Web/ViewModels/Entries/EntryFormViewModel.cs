using System.ComponentModel.DataAnnotations;
using Scriptorium_Nova.Web.Common;
using Scriptorium_Nova.Web.Common.Attributes;
using Scriptorium_Nova.Web.ViewModels.EntryTypes;

namespace Scriptorium_Nova.Web.ViewModels.Entries;

public class EntryFormViewModel
{
    [Required(ErrorMessage = "Please enter a name.")]
    [StringLength(
        EntityValidation.Entry.NameMaxLength,
        MinimumLength = EntityValidation.Entry.NameMinLength,
        ErrorMessage = "Name must be between {2} and {1} characters.")]
    [MustContainLetter]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Please enter a summary.")]
    [StringLength(
        EntityValidation.Entry.SummaryMaxLength,
        MinimumLength = EntityValidation.Entry.SummaryMinLength,
        ErrorMessage = "Summary must be between {2} and {1} characters.")]
    public string Summary { get; set; } = null!;

    [StringLength(
        EntityValidation.Entry.DetailsMaxLength,
        ErrorMessage = "Details cannot exceed {1} characters.")]
    public string? Details { get; set; }

    [Display(Name = "Type")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a type.")]
    public int EntryTypeId { get; set; }

    public IEnumerable<EntryTypeSelectViewModel> EntryTypes { get; set; } = new List<EntryTypeSelectViewModel>();
}