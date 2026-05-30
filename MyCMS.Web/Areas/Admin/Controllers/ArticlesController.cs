using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ArticlesController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IFileService _fileService;
        private readonly ICategoryService _categoryService;

        public ArticlesController(IArticleService articleService, IFileService fileService, ICategoryService categoryService)
        {
            _articleService = articleService;
            _fileService = fileService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var articles = await _articleService.GetAllArticlesAsync();
            return View(articles);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");
            var uploads = await _fileService.GetAllUploadsAsync();
            ViewData["Uploads"] = uploads;
            return View(new Article());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Article article, IFormFile? FeaturedImageFile)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");
                var uploads = await _fileService.GetAllUploadsAsync();
                ViewData["Uploads"] = uploads;
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

            var categories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");
            var uploads = await _fileService.GetAllUploadsAsync();
            ViewData["Uploads"] = uploads;
            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Article article, IFormFile? FeaturedImageFile)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");
                var uploads = await _fileService.GetAllUploadsAsync();
                ViewData["Uploads"] = uploads;
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
