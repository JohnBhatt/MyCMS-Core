using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MenusController : Controller
    {
        private readonly IMenuService _menuService;
        private readonly UserManager<ApplicationUser> _userManager;

        public MenusController(IMenuService menuService, UserManager<ApplicationUser> userManager)
        {
            _menuService = menuService;
            _userManager = userManager;
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

        public async Task<IActionResult> Items(Guid menuId)
        {
            var menu = await _menuService.GetMenuByIdAsync(menuId);
            if (menu == null)
            {
                return NotFound();
            }

            // Load menu items for this menu
            menu.MenuItems = await _menuService.GetMenuItemsByMenuIdAsync(menuId);

            ViewBag.MenuId = menuId;
            return View(menu);
        }

        public async Task<IActionResult> CreateItem(Guid menuId)
        {
            var menuItem = new MenuItem { MenuId = menuId };
            
            // Get existing menu items for this menu to populate parent dropdown
            var existingItems = await _menuService.GetMenuItemsByMenuIdAsync(menuId);
            ViewBag.MenuItems = existingItems;
            
            return View(menuItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateItem(MenuItem menuItem)
        {
            if (!ModelState.IsValid)
            {
                // Repopulate parent dropdown on validation error
                var existingItems = await _menuService.GetMenuItemsByMenuIdAsync(menuItem.MenuId);
                ViewBag.MenuItems = existingItems;
                return View(menuItem);
            }

            // Set CreatedBy to current user
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                menuItem.CreatedBy = currentUser.Id;
            }

            await _menuService.CreateMenuItemAsync(menuItem);
            return RedirectToAction(nameof(Items), new { menuId = menuItem.MenuId });
        }

        public async Task<IActionResult> EditItem(Guid id)
        {
            var menuItem = await _menuService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditItem(Guid id, MenuItem menuItem)
        {
            if (!ModelState.IsValid)
            {
                return View(menuItem);
            }

            // Set ModifiedBy to current user
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                menuItem.ModifiedBy = currentUser.Id;
                menuItem.ModifiedOn = DateTime.UtcNow;
            }

            await _menuService.UpdateMenuItemAsync(menuItem);
            return RedirectToAction(nameof(Items), new { menuId = menuItem.MenuId });
        }

        public async Task<IActionResult> DeleteItem(Guid id)
        {
            var menuItem = await _menuService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        [HttpPost, ActionName("DeleteItem")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItemConfirmed(Guid id)
        {
            var menuItem = await _menuService.GetMenuItemByIdAsync(id);
            if (menuItem != null)
            {
                await _menuService.DeleteMenuItemAsync(id);
                return RedirectToAction(nameof(Items), new { menuId = menuItem.MenuId });
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
