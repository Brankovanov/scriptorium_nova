using System;
using Scriptorium_Nova.Web.ViewModels.EntryTypes;

namespace Scriptorium_Nova.Web.Services.Interfaces;

public interface IEntryTypeService
{
    Task<IEnumerable<EntryTypeSelectViewModel>> GetAllAsync();

    Task<bool> ExistsAsync(int id);
}