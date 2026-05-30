using Microsoft.AspNetCore.Mvc;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IAmpService _ampService;
        private readonly IThemeService _themeService;

        public ArticleController(IArticleService articleService, IAmpService ampService, IThemeService themeService)
        {
            _articleService = articleService;
            _ampService = ampService;
            _themeService = themeService;
        }

        private async Task SetThemeLayout()
        {
            var activeTheme = await _themeService.GetActiveThemeAsync();
            string themeFolder = activeTheme?.FolderName ?? "Minimal";
            
            ViewBag.ThemeLayout = $"/Views/Themes/{themeFolder}/Shared/_Layout.cshtml";
            ViewBag.ThemeCss = $"/Themes/{themeFolder}/css/site.css";
        }

        public async Task<IActionResult> Index()
        {
            await SetThemeLayout();
            var articles = await _articleService.GetPublishedArticlesAsync();
            return View(articles);
        }

        public async Task<IActionResult> Details(string category, string slug)
        {
            await SetThemeLayout();
            var article = await _articleService.GetArticleByCategoryAndSlugAsync(category, slug);
            if (article == null)
            {
                return NotFound();
            }

            await _articleService.IncrementViewCountAsync(article.Id);
            var ampHtml = await _ampService.GenerateAmpPageAsync(article.Id);

            ViewBag.AmpHtml = ampHtml;
            return View(article);
        }

        public async Task<IActionResult> Amp(string category, string slug)
        {
            var article = await _articleService.GetArticleByCategoryAndSlugAsync(category, slug);
            if (article == null)
            {
                return NotFound();
            }

            var ampHtml = await _ampService.GenerateAmpPageAsync(article.Id);
            if (string.IsNullOrEmpty(ampHtml))
            {
                return NotFound();
            }

            ViewBag.AmpHtml = ampHtml;
            return View(article);
        }
    }
}
