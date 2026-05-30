using Microsoft.AspNetCore.Mvc;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPageService _pageService;
        private readonly IArticleService _articleService;
        private readonly IThemeService _themeService;

        public HomeController(IPageService pageService, IArticleService articleService, IThemeService themeService)
        {
            _pageService = pageService;
            _articleService = articleService;
            _themeService = themeService;
        }

        public async Task<IActionResult> Index()
        {
            var allPages = await _pageService.GetAllPagesAsync();
            var homePage = allPages.FirstOrDefault(p => p.IsHomePage);
            var featuredArticles = await _articleService.GetFeaturedArticlesAsync();
            var activeTheme = await _themeService.GetActiveThemeAsync();

            ViewBag.HomePage = homePage;
            ViewBag.FeaturedArticles = featuredArticles;
            
            // Set theme layout
            if (activeTheme != null)
            {
                ViewBag.ThemeLayout = $"/Views/Themes/{activeTheme.FolderName}/Shared/_Layout.cshtml";
                ViewBag.ThemeCss = $"/Themes/{activeTheme.FolderName}/css/site.css";
            }
            else
            {
                // Fallback to default Minimal theme
                ViewBag.ThemeLayout = "/Views/Themes/Minimal/Shared/_Layout.cshtml";
                ViewBag.ThemeCss = "/Themes/Minimal/css/site.css";
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error(int? statusCode = null)
        {
            var activeTheme = await _themeService.GetActiveThemeAsync();
            string themeFolder = activeTheme?.FolderName ?? "Minimal";
            
            ViewBag.ThemeLayout = $"/Views/Themes/{themeFolder}/Shared/_Layout.cshtml";
            ViewBag.ThemeCss = $"/Themes/{themeFolder}/css/site.css";

            if (statusCode == 404)
            {
                // Return theme-specific 404 page
                return View($"/Views/Themes/{themeFolder}/Shared/Error.cshtml");
            }

            // Return general error page
            return View();
        }
    }
}
