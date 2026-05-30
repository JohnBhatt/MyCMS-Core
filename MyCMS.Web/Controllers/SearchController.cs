using Microsoft.AspNetCore.Mvc;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchService _searchService;
        private readonly IThemeService _themeService;

        public SearchController(ISearchService searchService, IThemeService themeService)
        {
            _searchService = searchService;
            _themeService = themeService;
        }

        public async Task<IActionResult> Index(string q, int p = 1)
        {
            var results = await _searchService.SearchAsync(q, p, 10);
            var activeTheme = await _themeService.GetActiveThemeAsync();

            ViewBag.Query = q;
            ViewBag.ThemeLayout = activeTheme != null 
                ? $"/Views/Themes/{activeTheme.FolderName}/Shared/_Layout.cshtml" 
                : "/Views/Themes/Minimal/Shared/_Layout.cshtml";

            return View(results);
        }
    }
}
