using Microsoft.AspNetCore.Mvc;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IAmpService _ampService;

        public ArticleController(IArticleService articleService, IAmpService ampService)
        {
            _articleService = articleService;
            _ampService = ampService;
        }

        public async Task<IActionResult> Index()
        {
            var articles = await _articleService.GetPublishedArticlesAsync();
            return View(articles);
        }

        public async Task<IActionResult> Details(string slug)
        {
            var article = await _articleService.GetArticleBySlugAsync(slug);
            if (article == null)
            {
                return NotFound();
            }

            await _articleService.IncrementViewCountAsync(article.Id);
            var ampHtml = await _ampService.GenerateAmpPageAsync(article.Id);

            ViewBag.AmpHtml = ampHtml;
            return View(article);
        }

        public async Task<IActionResult> Amp(string slug)
        {
            var article = await _articleService.GetArticleBySlugAsync(slug);
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
