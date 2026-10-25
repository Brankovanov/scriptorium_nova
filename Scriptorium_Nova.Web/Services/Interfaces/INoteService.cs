using Scriptorium_Nova.Web.ViewModels.Notes;

namespace Scriptorium_Nova.Web.Services.Interfaces;

public interface INoteService
{
    Task<bool> EntryExistsAsync(int entryId);

    Task<int> CountByEntryAsync(int entryId);

    Task AddAsync(NoteFormViewModel model);

    Task<int?> DeleteAsync(int id);
}