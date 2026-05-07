using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Pages.Articles;

public class AmpModel : PageModel
{
    private readonly IArticleService _articleService;
    private readonly IAmpService _ampService;

    public AmpModel(IArticleService articleService, IAmpService ampService)
    {
        _articleService = articleService;
        _ampService = ampService;
    }

    public string AmpHtml { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(string slug)
    {
        var article = await _articleService.GetArticleBySlugAsync(slug);
        if (article == null)
        {
            return NotFound();
        }

        AmpHtml = await _ampService.GenerateAmpPageAsync(article.Id) ?? string.Empty;
        if (string.IsNullOrEmpty(AmpHtml))
        {
            return NotFound();
        }

        return Page();
    }
}
