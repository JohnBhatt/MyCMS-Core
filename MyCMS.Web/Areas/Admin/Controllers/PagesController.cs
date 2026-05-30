using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PageEntity = MyCMS.Core.Entities.Page;
using MyCMS.Core.Interfaces;

namespace MyCMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PagesController : Controller
    {
        private readonly IPageService _pageService;

        public PagesController(IPageService pageService)
        {
            _pageService = pageService;
        }

        public async Task<IActionResult> Index()
        {
            var pages = await _pageService.GetAllPagesAsync();
            return View(pages);
        }

        public async Task<IActionResult> Create()
        {
            var allPages = await _pageService.GetAllPagesAsync();
            ViewBag.ParentPages = new SelectList(allPages, "Id", "PageTitle");
            return View(new PageEntity());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PageEntity page)
        {
            if (!ModelState.IsValid)
            {
                var allPages = await _pageService.GetAllPagesAsync();
                ViewBag.ParentPages = new SelectList(allPages, "Id", "PageTitle");
                return View(page);
            }

            await _pageService.CreatePageAsync(page);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var page = await _pageService.GetPageByIdAsync(id);
            if (page == null)
            {
                return NotFound();
            }

            var allPages = await _pageService.GetAllPagesAsync();
            ViewBag.ParentPages = new SelectList(allPages.Where(p => p.Id != id), "Id", "PageTitle");
            return View(page);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PageEntity page)
        {
            if (!ModelState.IsValid)
            {
                var allPages = await _pageService.GetAllPagesAsync();
                ViewBag.ParentPages = new SelectList(allPages.Where(p => p.Id != id), "Id", "PageTitle");
                return View(page);
            }

            await _pageService.UpdatePageAsync(page);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var page = await _pageService.GetPageByIdAsync(id);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _pageService.DeletePageAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> SetHome(Guid id)
        {
            var page = await _pageService.GetPageByIdAsync(id);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        [HttpPost, ActionName("SetHome")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetHomeConfirmed(Guid id)
        {
            await _pageService.SetHomePageAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
