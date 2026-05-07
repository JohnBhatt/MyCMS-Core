using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Pages.OpenGraphTags
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IOpenGraphService _openGraphService;

        public IndexModel(IOpenGraphService openGraphService)
        {
            _openGraphService = openGraphService;
        }

        public List<OpenGraphTag> Tags { get; set; } = new List<OpenGraphTag>();

        public async Task OnGetAsync()
        {
            Tags = await _openGraphService.GetAllTagsAsync();
        }
    }
}
