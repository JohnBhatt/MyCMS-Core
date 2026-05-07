using Microsoft.EntityFrameworkCore;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;
using MyCMS.Data;

namespace MyCMS.Services
{
    public class PageService : IPageService
    {
        private readonly AppDbContext _context;

        public PageService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Page>> GetAllPagesAsync()
        {
            return await _context.Pages.ToListAsync();
        }

        public async Task<Page?> GetPageByIdAsync(Guid id)
        {
            return await _context.Pages.FindAsync(id);
        }

        public async Task<Page?> GetPageByUrlAsync(string url)
        {
            return await _context.Pages.FirstOrDefaultAsync(p => p.PageURL == url);
        }

        public async Task<Page> CreatePageAsync(Page page)
        {
            page.Id = Guid.NewGuid();
            page.CreatedOn = DateTime.UtcNow;
            _context.Pages.Add(page);
            await _context.SaveChangesAsync();
            return page;
        }

        public async Task<Page> UpdatePageAsync(Page page)
        {
            page.ModifiedOn = DateTime.UtcNow;
            _context.Pages.Update(page);
            await _context.SaveChangesAsync();
            return page;
        }

        public async Task DeletePageAsync(Guid id)
        {
            var page = await _context.Pages.FindAsync(id);
            if (page != null)
            {
                page.IsDeleted = true;
                page.ModifiedOn = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Page?> SetHomePageAsync(Guid id)
        {
            // Remove existing homepage
            var existingHomePage = await _context.Pages.FirstOrDefaultAsync(p => p.IsHomePage);
            if (existingHomePage != null)
            {
                existingHomePage.IsHomePage = false;
            }

            // Set new homepage
            var page = await _context.Pages.FindAsync(id);
            if (page != null)
            {
                page.IsHomePage = true;
                page.ModifiedOn = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            return page;
        }
    }
}
