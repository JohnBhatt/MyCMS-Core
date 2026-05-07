using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Pages.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IPageService _pageService;

        public IndexModel(IPageService pageService)
        {
            _pageService = pageService;
        }

        public List<Page> Pages { get; set; } = new List<Page>();

        public async Task OnGetAsync()
        {
            Pages = await _pageService.GetAllPagesAsync();
        }
    }
}
