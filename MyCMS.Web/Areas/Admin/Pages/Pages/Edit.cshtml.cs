using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Pages.Pages
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IPageService _pageService;

        public EditModel(IPageService pageService)
        {
            _pageService = pageService;
        }

        [BindProperty]
        public Page Page { get; set; } = new Page();

        public SelectList ParentPages { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var page = await _pageService.GetPageByIdAsync(id);
            if (page == null)
            {
                return NotFound();
            }

            Page = page;
            var allPages = await _pageService.GetAllPagesAsync();
            ParentPages = new SelectList(allPages.Where(p => p.Id != id), "Id", "PageTitle");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var allPages = await _pageService.GetAllPagesAsync();
                ParentPages = new SelectList(allPages.Where(p => p.Id != Page.Id), "Id", "PageTitle");
                return Page();
            }

            await _pageService.UpdatePageAsync(Page);
            return RedirectToPage("./Index");
        }
    }
}
