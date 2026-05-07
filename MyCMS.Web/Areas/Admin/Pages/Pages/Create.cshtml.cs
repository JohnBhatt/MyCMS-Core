using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Pages.Pages
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IPageService _pageService;

        public CreateModel(IPageService pageService)
        {
            _pageService = pageService;
        }

        [BindProperty]
        public Page Page { get; set; } = new Page();

        public SelectList ParentPages { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var allPages = await _pageService.GetAllPagesAsync();
            ParentPages = new SelectList(allPages, "Id", "PageTitle");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            await _pageService.CreatePageAsync(Page);
            return RedirectToPage("./Index");
        }
    }
}
