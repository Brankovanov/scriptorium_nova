using Microsoft.EntityFrameworkCore;
using Scriptorium_Nova.Web.Data;
using Scriptorium_Nova.Web.Data.Models;
using Scriptorium_Nova.Web.Services.Interfaces;
using Scriptorium_Nova.Web.ViewModels.Entries;

namespace Scriptorium_Nova.Web.Services.Implementations;

public class EntryService : IEntryService
{
    private readonly ApplicationDbContext context;

    public EntryService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<EntryListItemViewModel>> GetAllAsync(string? search, int? entryTypeId)
    {
        IQueryable<Entry> query = context.Entries.AsNoTracking();

        if (entryTypeId.HasValue)
        {
            query = query.Where(e => e.EntryTypeId == entryTypeId.Value);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            string term = search.Trim();
            query = query.Where(e => e.Name.Contains(term));
        }

        return await query
            .OrderBy(e => e.Name)
            .Select(e => new EntryListItemViewModel
            {
                Id = e.Id,
                Name = e.Name,
                Summary = e.Summary,
                EntryTypeName = e.EntryType.Name,
                CreatedOn = e.CreatedOn
            })
            .ToListAsync();
    }

    public async Task<EntryDetailsViewModel?> GetDetailsAsync(int id)
    {
        return await context.Entries
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new EntryDetailsViewModel
            {
                Id = e.Id,
                Name = e.Name,
                Summary = e.Summary,
                Details = e.Details,
                EntryTypeId = e.EntryTypeId,
                EntryTypeName = e.EntryType.Name,
                CreatedOn = e.CreatedOn
            })
            .FirstOrDefaultAsync();
    }

    public async Task<EntryFormViewModel?> GetForEditAsync(int id)
    {
        return await context.Entries
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new EntryFormViewModel
            {
                Name = e.Name,
                Summary = e.Summary,
                Details = e.Details,
                EntryTypeId = e.EntryTypeId
            })
            .FirstOrDefaultAsync();
    }

    public async Task<int> CreateAsync(EntryFormViewModel model)
    {
        var entry = new Entry
        {
            Name = model.Name.Trim(),
            Summary = model.Summary.Trim(),
            Details = model.Details?.Trim(),
            EntryTypeId = model.EntryTypeId
        };

        context.Entries.Add(entry);
        await context.SaveChangesAsync();

        return entry.Id;
    }

    public async Task<bool> EditAsync(int id, EntryFormViewModel model)
    {
        Entry? entry = await context.Entries.FindAsync(id);

        if (entry == null)
        {
            return false;
        }

        entry.Name = model.Name.Trim();
        entry.Summary = model.Summary.Trim();
        entry.Details = model.Details?.Trim();
        entry.EntryTypeId = model.EntryTypeId;

        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Entry? entry = await context.Entries.FindAsync(id);

        if (entry == null)
        {
            return false;
        }

        context.Entries.Remove(entry);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ExistsByNameAsync(string name, int entryTypeId, int? excludeId = null)
    {
        string trimmedName = name.Trim();

        return await context.Entries.AnyAsync(e =>
            e.EntryTypeId == entryTypeId &&
            e.Name == trimmedName &&
            (excludeId == null || e.Id != excludeId));
    }

    public async Task<EntryDeleteViewModel?> GetForDeleteAsync(int id)
    {
        return await context.Entries
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new EntryDeleteViewModel
            {
                Id = e.Id,
                Name = e.Name,
                Summary = e.Summary,
                EntryTypeName = e.EntryType.Name,
                NotesCount = e.Notes.Count
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> NameExistsInTypeAsync(string name, int entryTypeId, int? excludeEntryId = null)
    {
        string trimmed = name.Trim();

        return await context.Entries.AnyAsync(e =>
            e.EntryTypeId == entryTypeId &&
            e.Name == trimmed &&
            (excludeEntryId == null || e.Id != excludeEntryId));
    }
}