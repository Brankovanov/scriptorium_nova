using Scriptorium_Nova.Web.ViewModels.EntryTypes;

namespace Scriptorium_Nova.Web.ViewModels.Entries;

public class EntryIndexViewModel
{
    public IEnumerable<EntryListItemViewModel> Entries { get; set; } = new List<EntryListItemViewModel>();

    public IEnumerable<EntryTypeSelectViewModel> EntryTypes { get; set; } = new List<EntryTypeSelectViewModel>();

    public string? SearchTerm { get; set; }

    public int? EntryTypeId { get; set; }
}