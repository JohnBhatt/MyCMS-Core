using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Pages.OpenGraphTags
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly IOpenGraphService _openGraphService;

        public DeleteModel(IOpenGraphService openGraphService)
        {
            _openGraphService = openGraphService;
        }

        [BindProperty]
        public OpenGraphTag Tag { get; set; } = new OpenGraphTag();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var tag = await _openGraphService.GetTagByIdAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            Tag = tag;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            await _openGraphService.DeleteTagAsync(id);
            return RedirectToPage("./Index");
        }
    }
}
