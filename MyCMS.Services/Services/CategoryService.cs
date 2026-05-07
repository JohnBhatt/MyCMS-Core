using Microsoft.EntityFrameworkCore;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;
using MyCMS.Data;

namespace MyCMS.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
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
            return category;
        }

        public async Task<ArticleCategory> UpdateCategoryAsync(ArticleCategory category)
        {
            category.ModifiedOn = DateTime.UtcNow;
            _context.ArticleCategories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            var category = await _context.ArticleCategories.FindAsync(id);
            if (category != null)
            {
                category.IsDeleted = true;
                category.ModifiedOn = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
