using Microsoft.AspNetCore.Mvc;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Controllers
{
    public class PageController : Controller
    {
        private readonly IPageService _pageService;

        public PageController(IPageService pageService)
        {
            _pageService = pageService;
        }

        public async Task<IActionResult> Show(string url)
        {
            var page = await _pageService.GetPageByUrlAsync(url);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }
    }
}
