using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Pages;

public class FeedModel : PageModel
{
    private readonly IRssFeedService _rssFeedService;

    public FeedModel(IRssFeedService rssFeedService)
    {
        _rssFeedService = rssFeedService;
    }

    public string RssContent { get; set; } = string.Empty;

    public async Task OnGetAsync()
    {
        RssContent = await _rssFeedService.GenerateRssFeedAsync();
    }
}
