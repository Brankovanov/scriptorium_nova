using Microsoft.AspNetCore.Mvc;
using Scriptorium_Nova.Web.Services.Interfaces;
using Scriptorium_Nova.Web.ViewModels.EntryTypes;

namespace Scriptorium_Nova.Web.Controllers;

public class EntryTypesController : Controller
{
    private readonly IEntryTypeService entryTypeService;

    public EntryTypesController(IEntryTypeService entryTypeService)
    {
        this.entryTypeService = entryTypeService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        IEnumerable<EntryTypeListItemViewModel> model = await entryTypeService.GetAllWithCountAsync();

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        EntryTypeDetailsViewModel? model = await entryTypeService.GetDetailsAsync(id);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }
}