using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Pages.Pages
{
    [Authorize]
    public class SetHomeModel : PageModel
    {
        private readonly IPageService _pageService;

        public SetHomeModel(IPageService pageService)
        {
            _pageService = pageService;
        }

        [BindProperty]
        public Page Page { get; set; } = new Page();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var page = await _pageService.GetPageByIdAsync(id);
            if (page == null)
            {
                return NotFound();
            }

            Page = page;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            await _pageService.SetHomePageAsync(id);
            return RedirectToPage("./Index");
        }
    }
}
