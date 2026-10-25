using Scriptorium_Nova.Web.ViewModels.Entries;

namespace Scriptorium_Nova.Web.Services.Interfaces;

public interface IEntryService
{
    Task<IEnumerable<EntryListItemViewModel>> GetAllAsync(string? searchTerm = null, int? entryTypeId = null);

    Task<EntryDetailsViewModel?> GetDetailsAsync(int id);

    Task<EntryFormViewModel?> GetForEditAsync(int id);

    Task<EntryDeleteViewModel?> GetForDeleteAsync(int id);

    Task<int> CreateAsync(EntryFormViewModel model);

    Task<bool> EditAsync(int id, EntryFormViewModel model);

    Task<bool> DeleteAsync(int id);

    Task<bool> NameExistsInTypeAsync(string name, int entryTypeId, int? excludeEntryId = null);
}