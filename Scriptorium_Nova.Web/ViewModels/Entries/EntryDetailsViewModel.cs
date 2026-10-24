namespace Scriptorium_Nova.Web.ViewModels.Entries;

public class EntryDetailsViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Summary { get; set; } = null!;

    public string? Details { get; set; }

    public int EntryTypeId { get; set; }

    public string EntryTypeName { get; set; } = null!;

    public DateTime CreatedOn { get; set; }
}