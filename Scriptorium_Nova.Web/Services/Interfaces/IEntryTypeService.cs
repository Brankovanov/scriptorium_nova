using Scriptorium_Nova.Web.ViewModels.EntryTypes;

namespace Scriptorium_Nova.Web.Services.Interfaces;

public interface IEntryTypeService
{
    Task<IEnumerable<EntryTypeSelectViewModel>> GetAllAsync();

    Task<IEnumerable<EntryTypeListItemViewModel>> GetAllWithCountAsync();

    Task<EntryTypeDetailsViewModel?> GetDetailsAsync(int id);

    Task<bool> ExistsAsync(int id);
}