using System.ComponentModel.DataAnnotations;
using Scriptorium_Nova.Web.Common;
using Scriptorium_Nova.Web.ViewModels.EntryTypes;

namespace Scriptorium_Nova.Web.ViewModels.Entries;

public class EntryFormViewModel
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(
        EntityValidation.Entry.NameMaxLength,
        MinimumLength = EntityValidation.Entry.NameMinLength,
        ErrorMessage = "Name must be between {2} and {1} characters.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Summary is required.")]
    [StringLength(
        EntityValidation.Entry.SummaryMaxLength,
        MinimumLength = EntityValidation.Entry.SummaryMinLength,
        ErrorMessage = "Summary must be between {2} and {1} characters.")]
    public string Summary { get; set; } = null!;

    [StringLength(
        EntityValidation.Entry.DetailsMaxLength,
        ErrorMessage = "Details cannot exceed {1} characters.")]
    [DataType(DataType.MultilineText)]
    public string? Details { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Please select an entry type.")]
    [Display(Name = "Entry type")]
    public int EntryTypeId { get; set; }

    public IEnumerable<EntryTypeSelectViewModel> EntryTypes { get; set; }
        = new List<EntryTypeSelectViewModel>();
}s