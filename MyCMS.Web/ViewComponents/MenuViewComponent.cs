using Microsoft.AspNetCore.Mvc;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly IMenuService _menuService;

        public MenuViewComponent(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string position)
        {
            var menus = await _menuService.GetAllMenusAsync();
            var menu = menus.FirstOrDefault(m => m.Position == position);

            if (menu != null)
            {
                menu.MenuItems = await _menuService.GetMenuItemsByMenuIdAsync(menu.Id);
            }

            return View(menu);
        }
    }
}
