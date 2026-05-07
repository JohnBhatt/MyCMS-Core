using Microsoft.AspNetCore.Mvc;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Controllers
{
    public class FeedController : Controller
    {
        private readonly IRssFeedService _rssFeedService;

        public FeedController(IRssFeedService rssFeedService)
        {
            _rssFeedService = rssFeedService;
        }

        public async Task<IActionResult> Index()
        {
            var feed = await _rssFeedService.GenerateRssFeedAsync();
            return Content(feed, "application/rss+xml");
        }

        public async Task<IActionResult> Category(Guid id)
        {
            var feed = await _rssFeedService.GenerateCategoryRssFeedAsync(id);
            return Content(feed, "application/rss+xml");
        }

        public async Task<IActionResult> Tag(Guid id)
        {
            var feed = await _rssFeedService.GenerateTagRssFeedAsync(id);
            return Content(feed, "application/rss+xml");
        }
    }
}
