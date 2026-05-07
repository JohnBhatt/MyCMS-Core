using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IFileService _fileService;

        public CategoriesController(ICategoryService categoryService, IFileService fileService)
        {
            _categoryService = categoryService;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return View(categories);
        }

        public async Task<IActionResult> Create()
        {
            var uploads = await _fileService.GetAllUploadsAsync();
            ViewData["Uploads"] = uploads;
            return View(new ArticleCategory());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArticleCategory category, IFormFile? CategoryImageFile)
        {
            if (!ModelState.IsValid)
            {
                var uploads = await _fileService.GetAllUploadsAsync();
                ViewData["Uploads"] = uploads;
                return View(category);
            }

            if (CategoryImageFile != null)
            {
                var imagePath = await _fileService.SaveFileAsync(CategoryImageFile, "categories");
                category.CategoryImage = imagePath;
            }

            await _categoryService.CreateCategoryAsync(category);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            var uploads = await _fileService.GetAllUploadsAsync();
            ViewData["Uploads"] = uploads;
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ArticleCategory category, IFormFile? CategoryImageFile)
        {
            if (!ModelState.IsValid)
            {
                var uploads = await _fileService.GetAllUploadsAsync();
                ViewData["Uploads"] = uploads;
                return View(category);
            }

            if (CategoryImageFile != null)
            {
                var imagePath = await _fileService.SaveFileAsync(CategoryImageFile, "categories");
                category.CategoryImage = imagePath;
            }

            await _categoryService.UpdateCategoryAsync(category);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
