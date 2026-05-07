using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Pages.Articles;

public class DetailsModel : PageModel
{
    private readonly IArticleService _articleService;
    private readonly IAmpService _ampService;

    public DetailsModel(IArticleService articleService, IAmpService ampService)
    {
        _articleService = articleService;
        _ampService = ampService;
    }

    public Article? Article { get; set; }
    public string? AmpHtml { get; set; }

    public async Task<IActionResult> OnGetAsync(string slug)
    {
        Article = await _articleService.GetArticleBySlugAsync(slug);
        if (Article == null)
        {
            return NotFound();
        }

        await _articleService.IncrementViewCountAsync(Article.Id);
        AmpHtml = await _ampService.GenerateAmpPageAsync(Article.Id);
        return Page();
    }
}
