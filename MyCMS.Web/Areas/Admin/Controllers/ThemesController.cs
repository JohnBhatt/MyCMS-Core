using Microsoft.AspNetCore.Mvc;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ThemesController : Controller
    {
        private readonly IThemeService _themeService;

        public ThemesController(IThemeService themeService)
        {
            _themeService = themeService;
        }

        public async Task<IActionResult> Index()
        {
            var themes = await _themeService.GetAllThemesAsync();
            return View(themes);
        }

        public async Task<IActionResult> Activate(Guid id)
        {
            await _themeService.SetActiveThemeAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Configure(Guid id)
        {
            var theme = await _themeService.GetThemeByIdAsync(id);
            if (theme == null)
            {
                return NotFound();
            }

            var config = await _themeService.GetThemeConfigurationAsync(id, "homepage_categories");
            ViewBag.CategoryConfig = config?.Value;

            return View(theme);
        }

        [HttpPost]
        public async Task<IActionResult> Configure(Guid id, string categoryConfig)
        {
            await _themeService.SetThemeConfigurationAsync(id, "homepage_categories", categoryConfig);
            return RedirectToAction("Index");
        }
    }
}
