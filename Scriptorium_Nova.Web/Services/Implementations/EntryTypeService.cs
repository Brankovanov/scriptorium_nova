using Microsoft.EntityFrameworkCore;
using Scriptorium_Nova.Web.Data;
using Scriptorium_Nova.Web.Services.Interfaces;
using Scriptorium_Nova.Web.ViewModels.EntryTypes;

namespace Scriptorium_Nova.Web.Services.Implementations;

public class EntryTypeService : IEntryTypeService
{
    private readonly ApplicationDbContext context;

    public EntryTypeService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<EntryTypeSelectViewModel>> GetAllAsync()
    {
        return await context.EntryTypes
            .AsNoTracking()
            .OrderBy(t => t.Name)
            .Select(t => new EntryTypeSelectViewModel
            {
                Id = t.Id,
                Name = t.Name
            })
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await context.EntryTypes.AnyAsync(t => t.Id == id);
    }
}