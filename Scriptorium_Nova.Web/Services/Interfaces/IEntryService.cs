using Scriptorium_Nova.Web.ViewModels.Entries;

namespace Scriptorium_Nova.Web.Services.Interfaces;

public interface IEntryService
{
    Task<IEnumerable<EntryListItemViewModel>> GetAllAsync(string? search, int? entryTypeId);

    Task<EntryDetailsViewModel?> GetDetailsAsync(int id);

    Task<EntryFormViewModel?> GetForEditAsync(int id);

    Task<int> CreateAsync(EntryFormViewModel model);

    Task<bool> EditAsync(int id, EntryFormViewModel model);

    Task<bool> DeleteAsync(int id);

    Task<bool> ExistsByNameAsync(string name, int entryTypeId, int? excludeId = null);
}
