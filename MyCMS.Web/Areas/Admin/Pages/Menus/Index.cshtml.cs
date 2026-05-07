using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Pages.Menus
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IMenuService _menuService;

        public IndexModel(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public List<Menu> Menus { get; set; } = new List<Menu>();

        public async Task OnGetAsync()
        {
            Menus = await _menuService.GetAllMenusAsync();
        }
    }
}
