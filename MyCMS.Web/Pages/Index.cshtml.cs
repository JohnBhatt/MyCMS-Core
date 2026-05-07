using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Pages;

public class IndexModel : PageModel
{
    private readonly IPageService _pageService;
    private readonly IArticleService _articleService;

    public IndexModel(IPageService pageService, IArticleService articleService)
    {
        _pageService = pageService;
        _articleService = articleService;
    }

    public Page? HomePage { get; set; }
    public List<Article>? FeaturedArticles { get; set; }

    public async Task OnGetAsync()
    {
        var allPages = await _pageService.GetAllPagesAsync();
        HomePage = allPages.FirstOrDefault(p => p.IsHomePage);
        FeaturedArticles = await _articleService.GetFeaturedArticlesAsync();
    }
}
