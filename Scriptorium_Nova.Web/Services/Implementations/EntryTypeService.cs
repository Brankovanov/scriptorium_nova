using Microsoft.EntityFrameworkCore;
using Scriptorium_Nova.Web.Data;
using Scriptorium_Nova.Web.Services.Interfaces;
using Scriptorium_Nova.Web.ViewModels.EntryTypes;
using Scriptorium_Nova.Web.ViewModels.Entries;

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

    public async Task<IEnumerable<EntryTypeListItemViewModel>> GetAllWithCountAsync()
    {
        return await context.EntryTypes
            .AsNoTracking()
            .OrderBy(t => t.Name)
            .Select(t => new EntryTypeListItemViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                EntriesCount = t.Entries.Count
            })
            .ToListAsync();
    }

    public async Task<EntryTypeDetailsViewModel?> GetDetailsAsync(int id)
    {
        return await context.EntryTypes
            .AsNoTracking()
            .Where(t => t.Id == id)
            .Select(t => new EntryTypeDetailsViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Entries = t.Entries
                    .OrderBy(e => e.Name)
                    .Select(e => new EntryListItemViewModel
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Summary = e.Summary,
                        EntryTypeName = t.Name,
                        CreatedOn = e.CreatedOn
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }
}