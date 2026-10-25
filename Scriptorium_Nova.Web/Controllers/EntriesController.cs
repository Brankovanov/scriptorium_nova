using Microsoft.AspNetCore.Mvc;
using Scriptorium_Nova.Web.Services.Interfaces;
using Scriptorium_Nova.Web.ViewModels.Entries;

namespace Scriptorium_Nova.Web.Controllers;

public class EntriesController : Controller
{
    private readonly IEntryService entryService;
    private readonly IEntryTypeService entryTypeService;

    public EntriesController(IEntryService entryService, IEntryTypeService entryTypeService)
    {
        this.entryService = entryService;
        this.entryTypeService = entryTypeService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? searchTerm, int? entryTypeId)
    {
        EntryIndexViewModel model = new EntryIndexViewModel
        {
            Entries = await entryService.GetAllAsync(searchTerm, entryTypeId),
            EntryTypes = await entryTypeService.GetAllAsync(),
            SearchTerm = searchTerm,
            EntryTypeId = entryTypeId
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        EntryDetailsViewModel? model = await entryService.GetDetailsAsync(id);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        EntryFormViewModel model = new EntryFormViewModel
        {
            EntryTypes = await entryTypeService.GetAllAsync()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EntryFormViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (!await entryTypeService.ExistsAsync(model.EntryTypeId))
            {
                ModelState.AddModelError(nameof(model.EntryTypeId), "The selected type does not exist.");
            }
            else if (await entryService.NameExistsInTypeAsync(model.Name, model.EntryTypeId))
            {
                ModelState.AddModelError(nameof(model.Name), "An entry with this name already exists in the selected type.");
            }
        }

        if (!ModelState.IsValid)
        {
            model.EntryTypes = await entryTypeService.GetAllAsync();
            return View(model);
        }

        int id = await entryService.CreateAsync(model);

        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        EntryFormViewModel? model = await entryService.GetForEditAsync(id);

        if (model == null)
        {
            return NotFound();
        }

        model.EntryTypes = await entryTypeService.GetAllAsync();

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EntryFormViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (!await entryTypeService.ExistsAsync(model.EntryTypeId))
            {
                ModelState.AddModelError(nameof(model.EntryTypeId), "The selected type does not exist.");
            }
            else if (await entryService.NameExistsInTypeAsync(model.Name, model.EntryTypeId, id))
            {
                ModelState.AddModelError(nameof(model.Name), "An entry with this name already exists in the selected type.");
            }
        }

        if (!ModelState.IsValid)
        {
            model.EntryTypes = await entryTypeService.GetAllAsync();
            return View(model);
        }

        bool edited = await entryService.EditAsync(id, model);

        if (!edited)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        EntryDeleteViewModel? model = await entryService.GetForDeleteAsync(id);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost, ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        bool deleted = await entryService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }
}