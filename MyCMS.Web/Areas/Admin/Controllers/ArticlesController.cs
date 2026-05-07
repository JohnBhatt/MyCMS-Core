using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ArticlesController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IFileService _fileService;

        public ArticlesController(IArticleService articleService, IFileService fileService)
        {
            _articleService = articleService;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index()
        {
            var articles = await _articleService.GetAllArticlesAsync();
            return View(articles);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = Guid.NewGuid().ToString(), Text = "Category 1" },
                new SelectListItem { Value = Guid.NewGuid().ToString(), Text = "Category 2" }
            }, "Value", "Text");
            return View(new Article());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Article article, IFormFile? FeaturedImageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(new List<SelectListItem>
                {
                    new SelectListItem { Value = Guid.NewGuid().ToString(), Text = "Category 1" },
                    new SelectListItem { Value = Guid.NewGuid().ToString(), Text = "Category 2" }
                }, "Value", "Text");
                return View(article);
            }

            if (FeaturedImageFile != null)
            {
                var imagePath = await _fileService.SaveFileAsync(FeaturedImageFile, "articles");
                article.FeaturedImage = imagePath;
            }

            if (article.IsPublished && article.PublishedDate == null)
            {
                article.PublishedDate = DateTime.UtcNow;
            }

            await _articleService.CreateArticleAsync(article);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var article = await _articleService.GetArticleByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = Guid.NewGuid().ToString(), Text = "Category 1" },
                new SelectListItem { Value = Guid.NewGuid().ToString(), Text = "Category 2" }
            }, "Value", "Text");
            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Article article, IFormFile? FeaturedImageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(new List<SelectListItem>
                {
                    new SelectListItem { Value = Guid.NewGuid().ToString(), Text = "Category 1" },
                    new SelectListItem { Value = Guid.NewGuid().ToString(), Text = "Category 2" }
                }, "Value", "Text");
                return View(article);
            }

            if (FeaturedImageFile != null)
            {
                var imagePath = await _fileService.SaveFileAsync(FeaturedImageFile, "articles");
                article.FeaturedImage = imagePath;
            }

            if (article.IsPublished && article.PublishedDate == null)
            {
                article.PublishedDate = DateTime.UtcNow;
            }

            await _articleService.UpdateArticleAsync(article);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var article = await _articleService.GetArticleByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _articleService.DeleteArticleAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
