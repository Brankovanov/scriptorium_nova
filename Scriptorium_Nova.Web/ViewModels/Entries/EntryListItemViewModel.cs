namespace Scriptorium_Nova.Web.ViewModels.Entries;

public class EntryListItemViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Summary { get; set; } = null!;

    public string EntryTypeName { get; set; } = null!;

    public DateTime CreatedOn { get; set; }
}