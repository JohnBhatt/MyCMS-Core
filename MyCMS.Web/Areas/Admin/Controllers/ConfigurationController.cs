using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ConfigurationController : Controller
    {
        private readonly IConfigurationService _configService;
        private readonly IThemeService _themeService;

        public ConfigurationController(IConfigurationService configService, IThemeService themeService)
        {
            _configService = configService;
            _themeService = themeService;
        }

        public async Task<IActionResult> Index()
        {
            var config = await _configService.GetConfigurationAsync("Default");
            if (config != null)
            {
                ViewBag.Themes = await _themeService.GetAllThemesAsync();
                return View(config);
            }
            else
            {
                var newConfig = new MyCMS.Core.Entities.Configuration();
                newConfig.SectionName = "Default";
                newConfig.ThemeName = "Minimal"; // Set default theme
                ViewBag.Themes = await _themeService.GetAllThemesAsync();
                return View(newConfig);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(MyCMS.Core.Entities.Configuration config)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Themes = await _themeService.GetAllThemesAsync();
                return View(config);
            }

            config.SectionName = "Default";
            await _configService.UpdateConfigurationAsync(config);

            // Set the selected theme as active
            if (!string.IsNullOrEmpty(config.ThemeName))
            {
                var theme = await _themeService.GetThemeByNameAsync(config.ThemeName);
                if (theme != null)
                {
                    await _themeService.SetActiveThemeAsync(theme.Id);
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
