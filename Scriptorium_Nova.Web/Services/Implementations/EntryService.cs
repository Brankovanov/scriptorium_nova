using Microsoft.EntityFrameworkCore;
using Scriptorium_Nova.Web.Data;
using Scriptorium_Nova.Web.Data.Models;
using Scriptorium_Nova.Web.Services.Interfaces;
using Scriptorium_Nova.Web.ViewModels.Entries;
using Scriptorium_Nova.Web.ViewModels.Notes;

namespace Scriptorium_Nova.Web.Services.Implementations;

public class EntryService : IEntryService
{
    private readonly ApplicationDbContext context;

    public EntryService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<EntryListItemViewModel>> GetAllAsync(string? searchTerm = null, int? entryTypeId = null)
    {
        IQueryable<Entry> query = context.Entries.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            string term = searchTerm.Trim();
            query = query.Where(e => e.Name.Contains(term));
        }

        if (entryTypeId.HasValue)
        {
            query = query.Where(e => e.EntryTypeId == entryTypeId.Value);
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
                CreatedOn = e.CreatedOn,
                EntryTypeId = e.EntryTypeId,
                EntryTypeName = e.EntryType.Name,
                NewNote = new NoteFormViewModel { EntryId = e.Id },
                Notes = e.Notes
                    .OrderByDescending(n => n.CreatedOn)
                    .Select(n => new NoteViewModel
                    {
                        Id = n.Id,
                        Title = n.Title,
                        Text = n.Text,
                        CreatedOn = n.CreatedOn
                    })
                    .ToList()
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

    public async Task<int> CreateAsync(EntryFormViewModel model)
    {
        Entry entry = new Entry
        {
            Name = model.Name.Trim(),
            Summary = model.Summary.Trim(),
            Details = string.IsNullOrWhiteSpace(model.Details) ? null : model.Details.Trim(),
            EntryTypeId = model.EntryTypeId
        };

        await context.Entries.AddAsync(entry);
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
        entry.Details = string.IsNullOrWhiteSpace(model.Details) ? null : model.Details.Trim();
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

    public async Task<bool> NameExistsInTypeAsync(string name, int entryTypeId, int? excludeEntryId = null)
    {
        string trimmed = name.Trim();

        return await context.Entries.AnyAsync(e =>
            e.EntryTypeId == entryTypeId &&
            e.Name == trimmed &&
            (excludeEntryId == null || e.Id != excludeEntryId));
    }
}