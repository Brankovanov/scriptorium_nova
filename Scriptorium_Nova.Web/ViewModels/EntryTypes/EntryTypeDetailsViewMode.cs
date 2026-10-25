using Scriptorium_Nova.Web.ViewModels.Entries;

namespace Scriptorium_Nova.Web.ViewModels.EntryTypes;

public class EntryTypeDetailsViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public IEnumerable<EntryListItemViewModel> Entries { get; set; } = new List<EntryListItemViewModel>();
}