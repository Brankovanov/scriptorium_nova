namespace Scriptorium_Nova.Web.ViewModels.Notes;

public class NoteViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Text { get; set; } = null!;

    public DateTime CreatedOn { get; set; }
}