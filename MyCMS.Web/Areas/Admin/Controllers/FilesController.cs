using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class FilesController : Controller
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        public async Task<IActionResult> Index()
        {
            var uploads = await _fileService.GetAllUploadsAsync();
            return View(uploads);
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Please select a file to upload.");
                return View();
            }

            var upload = new Upload
            {
                DocumentName = file.FileName,
                FilePath = await _fileService.SaveFileAsync(file, folder),
                UniqueName = file.FileName,
                DocumentType = file.ContentType ?? "application/octet-stream"
            };

            await _fileService.UploadFileAsync(upload);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var upload = await _fileService.GetUploadByIdAsync(id);
            if (upload == null)
            {
                return NotFound();
            }

            return View(upload);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _fileService.DeleteUploadAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
