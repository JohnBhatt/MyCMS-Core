using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Pages.Menus
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly IMenuService _menuService;

        public DeleteModel(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [BindProperty]
        public Menu Menu { get; set; } = new Menu();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var menu = await _menuService.GetMenuByIdAsync(id);
            if (menu == null)
            {
                return NotFound();
            }

            Menu = menu;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            await _menuService.DeleteMenuAsync(id);
            return RedirectToPage("./Index");
        }
    }
}
