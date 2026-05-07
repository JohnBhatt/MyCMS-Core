using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Pages.Files
{
    [Authorize]
    public class UploadModel : PageModel
    {
        private readonly IFileService _fileService;

        public UploadModel(IFileService fileService)
        {
            _fileService = fileService;
        }

        [BindProperty]
        public Upload Upload { get; set; } = new Upload();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(IFormFile File, string Folder)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (File == null || File.Length == 0)
            {
                ModelState.AddModelError("File", "Please select a file to upload.");
                return Page();
            }

            // Save the file
            var filePath = await _fileService.SaveFileAsync(File, Folder);

            // Create upload record
            Upload.UniqueName = File.FileName;
            Upload.FilePath = filePath;
            await _fileService.UploadFileAsync(Upload);

            return RedirectToPage("./Index");
        }
    }
}
