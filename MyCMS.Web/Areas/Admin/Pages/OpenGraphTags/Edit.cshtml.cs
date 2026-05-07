using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Pages.OpenGraphTags
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IOpenGraphService _openGraphService;

        public EditModel(IOpenGraphService openGraphService)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _openGraphService.UpdateTagAsync(Tag);
            return RedirectToPage("./Index");
        }
    }
}
