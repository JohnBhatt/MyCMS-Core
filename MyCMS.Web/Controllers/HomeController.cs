using Microsoft.AspNetCore.Mvc;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPageService _pageService;
        private readonly IArticleService _articleService;

        public HomeController(IPageService pageService, IArticleService articleService)
        {
            _pageService = pageService;
            _articleService = articleService;
        }

        public async Task<IActionResult> Index()
        {
            var allPages = await _pageService.GetAllPagesAsync();
            var homePage = allPages.FirstOrDefault(p => p.IsHomePage);
            var featuredArticles = await _articleService.GetFeaturedArticlesAsync();

            ViewBag.HomePage = homePage;
            ViewBag.FeaturedArticles = featuredArticles;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
