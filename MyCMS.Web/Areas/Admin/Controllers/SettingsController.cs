using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SettingsController : Controller
    {
        private readonly ISettingService _settingService;

        public SettingsController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public async Task<IActionResult> Index(string category = null)
        {
            var settings = string.IsNullOrEmpty(category)
                ? await _settingService.GetAllAsync()
                : await _settingService.GetByCategoryAsync(category);

            ViewBag.Categories = new[] { "General", "Social", "SEO", "Analytics" };
            ViewBag.CurrentCategory = category;
            return View(settings);
        }

        public IActionResult Create()
        {
            return View(new Setting { Category = "General", IsEditable = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Setting setting, string changeReason)
        {
            if (!ModelState.IsValid)
                return View(setting);

            try
            {
                await _settingService.CreateAsync(setting, changeReason);
                return RedirectToAction(nameof(Index), new { category = setting.Category });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(setting);
            }
        }

        public async Task<IActionResult> Edit(string key)
        {
            var setting = await _settingService.GetByKeyAsync(key);
            if (setting == null)
                return NotFound();

            return View(setting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Setting setting, string changeReason)
        {
            if (!ModelState.IsValid)
                return View(setting);

            try
            {
                await _settingService.UpdateAsync(setting, changeReason);
                return RedirectToAction(nameof(Index), new { category = setting.Category });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(setting);
            }
        }

        public async Task<IActionResult> AuditLog(string key, int count = 50)
        {
            var logs = await _settingService.GetAuditLogsAsync(key, count);
            ViewBag.SettingKey = key;
            return View(logs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string key, string changeReason)
        {
            try
            {
                await _settingService.DeleteAsync(key, changeReason);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
