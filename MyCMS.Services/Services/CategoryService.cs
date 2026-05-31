using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;
using MyCMS.Data;

namespace MyCMS.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        private readonly IAuditService _auditService;

        public CategoryService(AppDbContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        public async Task<List<ArticleCategory>> GetAllCategoriesAsync()
        {
            return await _context.ArticleCategories.ToListAsync();
        }

        public async Task<ArticleCategory?> GetCategoryByIdAsync(Guid id)
        {
            return await _context.ArticleCategories.FindAsync(id);
        }

        public async Task<ArticleCategory> CreateCategoryAsync(ArticleCategory category)
        {
            category.Id = Guid.NewGuid();
            category.CreatedOn = DateTime.UtcNow;
            _context.ArticleCategories.Add(category);
            await _context.SaveChangesAsync();
            
            await _auditService.LogAsync("Categories", category.Id.ToString(), "Created", null,
                JsonSerializer.Serialize(new { category.CategoryName, category.Slug, category.ParentCategory }), "Category created");
            return category;
        }

        public async Task<ArticleCategory> UpdateCategoryAsync(ArticleCategory category)
        {
            var existing = await _context.ArticleCategories.FindAsync(category.Id);
            if (existing == null) throw new InvalidOperationException("Category not found");
            
            var oldValues = JsonSerializer.Serialize(new { existing.CategoryName, existing.Slug, existing.ParentCategory });
            
            category.ModifiedOn = DateTime.UtcNow;
            _context.ArticleCategories.Update(category);
            await _context.SaveChangesAsync();
            
            var newValues = JsonSerializer.Serialize(new { category.CategoryName, category.Slug, category.ParentCategory });
            await _auditService.LogAsync("Categories", category.Id.ToString(), "Updated", oldValues, newValues, "Category updated");
            return category;
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            var category = await _context.ArticleCategories.FindAsync(id);
            if (category != null)
            {
                var oldValues = JsonSerializer.Serialize(new { category.CategoryName, category.Slug });
                category.IsDeleted = true;
                category.ModifiedOn = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                
                await _auditService.LogAsync("Categories", id.ToString(), "Deleted", oldValues, null, "Category deleted");
            }
        }
    }
}
