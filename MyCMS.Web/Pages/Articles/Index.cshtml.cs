using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Pages.Articles;

public class IndexModel : PageModel
{
    private readonly IArticleService _articleService;

    public IndexModel(IArticleService articleService)
    {
        _articleService = articleService;
    }

    public List<Article> Articles { get; set; } = new List<Article>();

    public async Task OnGetAsync()
    {
        Articles = await _articleService.GetPublishedArticlesAsync();
    }
}
