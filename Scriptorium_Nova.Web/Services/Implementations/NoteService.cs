using Microsoft.EntityFrameworkCore;
using Scriptorium_Nova.Web.Data;
using Scriptorium_Nova.Web.Data.Models;
using Scriptorium_Nova.Web.Services.Interfaces;
using Scriptorium_Nova.Web.ViewModels.Notes;

namespace Scriptorium_Nova.Web.Services.Implementations;

public class NoteService : INoteService
{
    private readonly ApplicationDbContext context;

    public NoteService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<bool> EntryExistsAsync(int entryId)
    {
        return await context.Entries.AnyAsync(e => e.Id == entryId);
    }

    public async Task<int> CountByEntryAsync(int entryId)
    {
        return await context.Notes.CountAsync(n => n.EntryId == entryId);
    }

    public async Task AddAsync(NoteFormViewModel model)
    {
        Note note = new Note
        {
            Title = model.Title.Trim(),
            Text = model.Text.Trim(),
            EntryId = model.EntryId
        };

        await context.Notes.AddAsync(note);
        await context.SaveChangesAsync();
    }

    public async Task<int?> DeleteAsync(int id)
    {
        Note? note = await context.Notes.FindAsync(id);

        if (note == null)
        {
            return null;
        }

        int entryId = note.EntryId;

        context.Notes.Remove(note);
        await context.SaveChangesAsync();

        return entryId;
    }
}