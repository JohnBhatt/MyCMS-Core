using Microsoft.AspNetCore.Mvc;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Controllers
{
    public class SitemapController : Controller
    {
        private readonly ISitemapService _sitemapService;

        public SitemapController(ISitemapService sitemapService)
        {
            _sitemapService = sitemapService;
        }

        public async Task<IActionResult> Index()
        {
            var sitemap = await _sitemapService.GenerateSitemapAsync();
            return Content(sitemap, "application/xml");
        }
    }
}
