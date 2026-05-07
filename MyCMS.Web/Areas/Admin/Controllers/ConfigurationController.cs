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

        public ConfigurationController(IConfigurationService configService)
        {
            _configService = configService;
        }

        public async Task<IActionResult> Index()
        {
            var config = await _configService.GetConfigurationAsync("Default");
            if (config != null)
            {
                return View(config);
            }
            else
            {
                var newConfig = new MyCMS.Core.Entities.Configuration();
                newConfig.SectionName = "Default";
                return View(newConfig);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(MyCMS.Core.Entities.Configuration config)
        {
            if (!ModelState.IsValid)
            {
                return View(config);
            }

            config.SectionName = "Default";
            await _configService.UpdateConfigurationAsync(config);
            return RedirectToAction(nameof(Index));
        }
    }
}
