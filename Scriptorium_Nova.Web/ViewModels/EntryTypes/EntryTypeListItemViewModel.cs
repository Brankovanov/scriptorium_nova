namespace Scriptorium_Nova.Web.ViewModels.EntryTypes;

public class EntryTypeListItemViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int EntriesCount { get; set; }
}