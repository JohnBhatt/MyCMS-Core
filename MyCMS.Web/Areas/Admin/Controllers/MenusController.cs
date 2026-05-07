using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class MenusController : Controller
    {
        private readonly IMenuService _menuService;

        public MenusController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public async Task<IActionResult> Index()
        {
            var menus = await _menuService.GetAllMenusAsync();
            return View(menus);
        }

        public IActionResult Create()
        {
            return View(new Menu());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Menu menu)
        {
            if (!ModelState.IsValid)
            {
                return View(menu);
            }

            await _menuService.CreateMenuAsync(menu);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var menu = await _menuService.GetMenuByIdAsync(id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Menu menu)
        {
            if (!ModelState.IsValid)
            {
                return View(menu);
            }

            await _menuService.UpdateMenuAsync(menu);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var menu = await _menuService.GetMenuByIdAsync(id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _menuService.DeleteMenuAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Items(Guid id)
        {
            var menu = await _menuService.GetMenuByIdAsync(id);
            if (menu == null)
            {
                return NotFound();
            }

            ViewBag.MenuId = id;
            return View(menu);
        }
    }
}
