using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Pages.Files
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly IFileService _fileService;

        public DeleteModel(IFileService fileService)
        {
            _fileService = fileService;
        }

        [BindProperty]
        public Upload Upload { get; set; } = new Upload();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var upload = await _fileService.GetUploadByIdAsync(id);
            if (upload == null)
            {
                return NotFound();
            }

            Upload = upload;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            await _fileService.DeleteUploadAsync(id);
            return RedirectToPage("./Index");
        }
    }
}
