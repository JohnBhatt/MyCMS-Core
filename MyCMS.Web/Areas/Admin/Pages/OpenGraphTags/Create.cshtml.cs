using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Pages.OpenGraphTags
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IOpenGraphService _openGraphService;

        public CreateModel(IOpenGraphService openGraphService)
        {
            _openGraphService = openGraphService;
        }

        [BindProperty]
        public OpenGraphTag Tag { get; set; } = new OpenGraphTag();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _openGraphService.CreateTagAsync(Tag);
            return RedirectToPage("./Index");
        }
    }
}
