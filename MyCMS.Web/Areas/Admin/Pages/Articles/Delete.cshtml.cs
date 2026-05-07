using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Pages.Articles
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly IArticleService _articleService;

        public DeleteModel(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [BindProperty]
        public Article Article { get; set; } = new Article();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var article = await _articleService.GetArticleByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            Article = article;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            await _articleService.DeleteArticleAsync(id);
            return RedirectToPage("./Index");
        }
    }
}
