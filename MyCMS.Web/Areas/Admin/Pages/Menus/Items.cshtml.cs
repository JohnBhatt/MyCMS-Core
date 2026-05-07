using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Pages.Menus
{
    [Authorize]
    public class ItemsModel : PageModel
    {
        private readonly IMenuService _menuService;

        public ItemsModel(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public Guid MenuId { get; set; }
        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        public async Task OnGetAsync(Guid menuId)
        {
            MenuId = menuId;
            MenuItems = await _menuService.GetMenuItemsByMenuIdAsync(menuId);
        }
    }
}
