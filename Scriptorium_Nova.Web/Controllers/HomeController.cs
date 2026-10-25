using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Scriptorium_Nova.Web.ViewModels.Shared;
using Scriptorium_Nova.Web.Services.Interfaces;
using Scriptorium_Nova.Web.ViewModels.Home;

namespace Scriptorium_Nova.Web.Controllers;

public class HomeController : Controller
{
    private const int RecentEntriesCount = 6;

    private readonly IEntryService entryService;
    private readonly IEntryTypeService entryTypeService;

    public HomeController(IEntryService entryService, IEntryTypeService entryTypeService)
    {
        this.entryService = entryService;
        this.entryTypeService = entryTypeService;
    }

    public async Task<IActionResult> Index()
    {
        HomeIndexViewModel model = new HomeIndexViewModel
        {
            RecentEntries = await entryService.GetRecentAsync(RecentEntriesCount),
            EntryTypes = await entryTypeService.GetAllWithCountAsync()
        };

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public IActionResult HttpStatus(int code)
    {
        if (code == 404)
        {
            return View("NotFound");
        }

        return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
