using Microsoft.EntityFrameworkCore;
using MyCMS.Core.Entities;
using MyCMS.Core.Helpers;
using MyCMS.Core.Interfaces;
using MyCMS.Data;

namespace MyCMS.Services
{
    public class ArticleService : IArticleService
    {
        private readonly AppDbContext _context;

        public ArticleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Article>> GetAllArticlesAsync()
        {
            return await _context.Articles.ToListAsync();
        }

        public async Task<List<Article>> GetPublishedArticlesAsync()
        {
            return await _context.Articles
                .Where(a => a.IsPublished)
                .OrderByDescending(a => a.PublishedDate)
                .ToListAsync();
        }

        public async Task<List<Article>> GetArticlesByCategoryAsync(Guid categoryId)
        {
            return await _context.Articles
                .Where(a => a.CategoryId == categoryId && a.IsPublished)
                .OrderByDescending(a => a.PublishedDate)
                .ToListAsync();
        }

        public async Task<List<Article>> GetArticlesByTagAsync(Guid tagId)
        {
            return await _context.Articles
                .Where(a => a.ArticleTagMappings.Any(atm => atm.TagId == tagId) && a.IsPublished)
                .OrderByDescending(a => a.PublishedDate)
                .ToListAsync();
        }

        public async Task<List<Article>> GetFeaturedArticlesAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.Articles
                .Where(a => a.IsFeatured && a.IsPublished &&
                            a.FeaturedFrom <= now && a.FeaturedUpto >= now)
                .OrderByDescending(a => a.PublishedDate)
                .ToListAsync();
        }

        public async Task<Article?> GetArticleByIdAsync(Guid id)
        {
            return await _context.Articles.FindAsync(id);
        }

        public async Task<Article?> GetArticleBySlugAsync(string slug)
        {
            var sanitizedSlug = SlugHelper.SanitizeSlug(slug);
            return await _context.Articles.FirstOrDefaultAsync(a => a.Slug == sanitizedSlug);
        }

        public async Task<Article?> GetArticleByCategoryAndSlugAsync(string category, string slug)
        {
            var sanitizedCategory = SlugHelper.SanitizeSlug(category);
            var sanitizedSlug = SlugHelper.SanitizeSlug(slug);
            return await _context.Articles
                .Include(a => a.Category)
                .FirstOrDefaultAsync(a => a.Category != null && 
                    a.Category.Slug == sanitizedCategory && 
                    a.Slug == sanitizedSlug);
        }

        public async Task<Article> CreateArticleAsync(Article article)
        {
            article.Id = Guid.NewGuid();
            article.CreatedOn = DateTime.UtcNow;
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
            return article;
        }

        public async Task<Article> UpdateArticleAsync(Article article)
        {
            article.Slug = SlugHelper.GenerateSlug(article.Slug);
            article.ModifiedOn = DateTime.UtcNow;
            _context.Articles.Update(article);
            await _context.SaveChangesAsync();
            return article;
        }

        public async Task DeleteArticleAsync(Guid id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                article.IsDeleted = true;
                article.ModifiedOn = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task IncrementViewCountAsync(Guid id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                article.ViewCount++;
                await _context.SaveChangesAsync();
            }
        }
    }
}
