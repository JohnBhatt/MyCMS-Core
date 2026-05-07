using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OpenGraphTagsController : Controller
    {
        private readonly IOpenGraphService _openGraphService;

        public OpenGraphTagsController(IOpenGraphService openGraphService)
        {
            _openGraphService = openGraphService;
        }

        public async Task<IActionResult> Index()
        {
            var tags = await _openGraphService.GetAllTagsAsync();
            return View(tags);
        }

        public IActionResult Create()
        {
            return View(new OpenGraphTag());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OpenGraphTag tag)
        {
            if (!ModelState.IsValid)
            {
                return View(tag);
            }

            await _openGraphService.CreateTagAsync(tag);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await _openGraphService.GetTagByIdAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, OpenGraphTag tag)
        {
            if (!ModelState.IsValid)
            {
                return View(tag);
            }

            await _openGraphService.UpdateTagAsync(tag);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var tag = await _openGraphService.GetTagByIdAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _openGraphService.DeleteTagAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
