using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Pages;

public class SitemapModel : PageModel
{
    private readonly ISitemapService _sitemapService;

    public SitemapModel(ISitemapService sitemapService)
    {
        _sitemapService = sitemapService;
    }

    public string SitemapContent { get; set; } = string.Empty;

    public async Task OnGetAsync()
    {
        SitemapContent = await _sitemapService.GenerateSitemapAsync();
    }
}
