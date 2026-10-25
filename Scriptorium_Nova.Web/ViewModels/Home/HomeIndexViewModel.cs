using Scriptorium_Nova.Web.ViewModels.Entries;
using Scriptorium_Nova.Web.ViewModels.EntryTypes;

namespace Scriptorium_Nova.Web.ViewModels.Home;

public class HomeIndexViewModel
{
    public IEnumerable<EntryListItemViewModel> RecentEntries { get; set; } = new List<EntryListItemViewModel>();

    public IEnumerable<EntryTypeListItemViewModel> EntryTypes { get; set; } = new List<EntryTypeListItemViewModel>();
}