using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;
using MyCMS.Data;

namespace MyCMS.Services
{
    public class PageService : IPageService
    {
        private readonly AppDbContext _context;
        private readonly IAuditService _auditService;

        public PageService(AppDbContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
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
            
            await _auditService.LogAsync("Pages", page.Id.ToString(), "Created", null,
                JsonSerializer.Serialize(new { page.PageTitle, page.PageURL, page.IsHomePage }), "Page created");
            return page;
        }

        public async Task<Page> UpdatePageAsync(Page page)
        {
            var existing = await _context.Pages.FindAsync(page.Id);
            if (existing == null) throw new InvalidOperationException("Page not found");
            
            var oldValues = JsonSerializer.Serialize(new { existing.PageTitle, existing.PageURL, existing.IsHomePage, existing.PageBody });
            
            page.ModifiedOn = DateTime.UtcNow;
            _context.Pages.Update(page);
            await _context.SaveChangesAsync();
            
            var newValues = JsonSerializer.Serialize(new { page.PageTitle, page.PageURL, page.IsHomePage, page.PageBody });
            await _auditService.LogAsync("Pages", page.Id.ToString(), "Updated", oldValues, newValues, "Page updated");
            return page;
        }

        public async Task DeletePageAsync(Guid id)
        {
            var page = await _context.Pages.FindAsync(id);
            if (page != null)
            {
                var oldValues = JsonSerializer.Serialize(new { page.PageTitle, page.PageURL, page.IsHomePage });
                page.IsDeleted = true;
                page.ModifiedOn = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                
                await _auditService.LogAsync("Pages", id.ToString(), "Deleted", oldValues, null, "Page deleted");
            }
        }

        public async Task<Page?> SetHomePageAsync(Guid id)
        {
            // Remove existing homepage
            var existingHomePage = await _context.Pages.FirstOrDefaultAsync(p => p.IsHomePage);
            if (existingHomePage != null)
            {
                existingHomePage.IsHomePage = false;
                await _auditService.LogAsync("Pages", existingHomePage.Id.ToString(), "Updated", 
                    JsonSerializer.Serialize(new { IsHomePage = true }),
                    JsonSerializer.Serialize(new { IsHomePage = false }), "Homepage changed");
            }

            // Set new homepage
            var page = await _context.Pages.FindAsync(id);
            if (page != null)
            {
                page.IsHomePage = true;
                page.ModifiedOn = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                
                await _auditService.LogAsync("Pages", id.ToString(), "Updated",
                    JsonSerializer.Serialize(new { IsHomePage = false }),
                    JsonSerializer.Serialize(new { IsHomePage = true }), "Set as homepage");
            }
            return page;
        }
    }
}
