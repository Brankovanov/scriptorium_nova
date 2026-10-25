using Microsoft.AspNetCore.Mvc;
using Scriptorium_Nova.Web.Common;
using Scriptorium_Nova.Web.Services.Interfaces;
using Scriptorium_Nova.Web.ViewModels.Entries;
using Scriptorium_Nova.Web.ViewModels.Notes;

namespace Scriptorium_Nova.Web.Controllers;

public class NotesController : Controller
{
    private readonly INoteService noteService;
    private readonly IEntryService entryService;

    public NotesController(INoteService noteService, IEntryService entryService)
    {
        this.noteService = noteService;
        this.entryService = entryService;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(NoteFormViewModel model)
    {
        if (!await noteService.EntryExistsAsync(model.EntryId))
        {
            return NotFound();
        }

        if (ModelState.IsValid &&
            await noteService.CountByEntryAsync(model.EntryId) >= EntityValidation.Note.MaxNotesPerEntry)
        {
            ModelState.AddModelError(
                string.Empty,
                $"An entry can have at most {EntityValidation.Note.MaxNotesPerEntry} notes.");
        }

        if (!ModelState.IsValid)
        {
            EntryDetailsViewModel? details = await entryService.GetDetailsAsync(model.EntryId);

            if (details == null)
            {
                return NotFound();
            }

            details.NewNote = model;

            return View("~/Views/Entries/Details.cshtml", details);
        }

        await noteService.AddAsync(model);

        return RedirectToAction("Details", "Entries", new { id = model.EntryId }, "notes");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        int? entryId = await noteService.DeleteAsync(id);

        if (entryId == null)
        {
            return NotFound();
        }

        return RedirectToAction("Details", "Entries", new { id = entryId.Value }, "notes");
    }
}