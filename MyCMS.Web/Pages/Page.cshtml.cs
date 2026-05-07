using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Pages;

public class PageModel : PageModel
{
    private readonly IPageService _pageService;

    public PageModel(IPageService pageService)
    {
        _pageService = pageService;
    }

    public Core.Entities.Page? Page { get; set; }

    public async Task<IActionResult> OnGetAsync(string url)
    {
        Page = await _pageService.GetPageByUrlAsync(url);
        if (Page == null)
        {
            return NotFound();
        }

        return Page();
    }
}
