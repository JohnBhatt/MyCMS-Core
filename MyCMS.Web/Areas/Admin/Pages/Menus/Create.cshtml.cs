using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Pages.Menus
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IMenuService _menuService;

        public CreateModel(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [BindProperty]
        public Menu Menu { get; set; } = new Menu();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _menuService.CreateMenuAsync(Menu);
            return RedirectToPage("./Index");
        }
    }
}
