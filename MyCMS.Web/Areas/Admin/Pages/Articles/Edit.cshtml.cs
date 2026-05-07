using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Pages.Articles
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IArticleService _articleService;
        private readonly IFileService _fileService;

        public EditModel(IArticleService articleService, IFileService fileService)
        {
            _articleService = articleService;
            _fileService = fileService;
        }

        [BindProperty]
        public Article Article { get; set; } = new Article();

        public SelectList Categories { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var article = await _articleService.GetArticleByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            Article = article;
            Categories = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = Guid.NewGuid().ToString(), Text = "Category 1" },
                new SelectListItem { Value = Guid.NewGuid().ToString(), Text = "Category 2" }
            }, "Value", "Text");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile? FeaturedImageFile)
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(Article.Id);
                return Page();
            }

            // Handle featured image upload
            if (FeaturedImageFile != null)
            {
                var imagePath = await _fileService.SaveFileAsync(FeaturedImageFile, "articles");
                Article.FeaturedImage = imagePath;
            }

            // Update published date if just published
            if (Article.IsPublished && Article.PublishedDate == null)
            {
                Article.PublishedDate = DateTime.UtcNow;
            }

            await _articleService.UpdateArticleAsync(Article);
            return RedirectToPage("./Index");
        }
    }
}
