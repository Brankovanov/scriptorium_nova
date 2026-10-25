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

    public async Task<IActionResult> Index(string? search, int? entryTypeId)
    {
        var entries = await entryService.GetAllAsync(search, entryTypeId);

        return View(entries);
    }

    public async Task<IActionResult> Details(int id)
    {
        var model = await entryService.GetDetailsAsync(id);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new EntryFormViewModel();
        await FillEntryTypesAsync(model);

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
                ModelState.AddModelError(nameof(model.EntryTypeId), "The selected entry type does not exist.");
            }
            else if (await entryService.NameExistsInTypeAsync(model.Name, model.EntryTypeId))
            {
                ModelState.AddModelError(nameof(model.Name), "An entry with this name already exists in this type.");
            }
        }

        if (!ModelState.IsValid)
        {
            await FillEntryTypesAsync(model);

            return View(model);
        }

        int id = await entryService.CreateAsync(model);

        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await entryService.GetForEditAsync(id);

        if (model == null)
        {
            return NotFound();
        }

        await FillEntryTypesAsync(model);

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
                ModelState.AddModelError(nameof(model.EntryTypeId), "The selected entry type does not exist.");
            }
            else if (await entryService.NameExistsInTypeAsync(model.Name, model.EntryTypeId, id))
            {
                ModelState.AddModelError(nameof(model.Name), "An entry with this name already exists in this type.");
            }
        }

        if (!ModelState.IsValid)
        {
            await FillEntryTypesAsync(model);

            return View(model);
        }

        bool updated = await entryService.EditAsync(id, model);

        if (!updated)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var model = await entryService.GetDetailsAsync(id);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        bool deleted = await entryService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task FillEntryTypesAsync(EntryFormViewModel model)
    {
        model.EntryTypes = await entryTypeService.GetAllAsync();
    }
}