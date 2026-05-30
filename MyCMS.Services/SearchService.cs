using Microsoft.EntityFrameworkCore;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;
using MyCMS.Data;

namespace MyCMS.Services
{
    public class SearchService : ISearchService
    {
        private readonly AppDbContext _context;

        public SearchService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SearchResult> SearchAsync(string query, int page = 1, int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return new SearchResult { CurrentPage = page, PageSize = pageSize };
            }

            var searchTerm = query.Trim();

            // Full-text search for Articles (PostgreSQL)
            var articlesQuery = _context.Articles
                .Where(a => !a.IsDeleted && a.PublishedDate != null && a.PublishedDate <= DateTime.UtcNow)
                .Where(a => 
                    EF.Functions.ILike(a.Title, $"%{searchTerm}%") ||
                    EF.Functions.ILike(a.Content, $"%{searchTerm}%") ||
                    EF.Functions.ILike(a.MetaDescription, $"%{searchTerm}%"));

            var articles = await articlesQuery
                .OrderByDescending(a => a.PublishedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new ArticleSearchResult
                {
                    Id = a.Id,
                    Title = a.Title,
                    Slug = a.Slug,
                    MetaDescription = a.MetaDescription,
                    FeaturedImage = a.FeaturedImage,
                    CategoryName = a.Category != null ? a.Category.CategoryName : null,
                    PublishedDate = a.PublishedDate
                })
                .ToListAsync();

            // Full-text search for Pages
            var pagesQuery = _context.Pages
                .Where(p => !p.IsDeleted)
                .Where(p => 
                    EF.Functions.ILike(p.PageTitle, $"%{searchTerm}%") ||
                    EF.Functions.ILike(p.PageBody, $"%{searchTerm}%") ||
                    EF.Functions.ILike(p.PageSummary, $"%{searchTerm}%"));

            var pages = await pagesQuery
                .OrderByDescending(p => p.CreatedOn)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PageSearchResult
                {
                    Id = p.Id,
                    Title = p.PageTitle,
                    Slug = p.PageURL,
                    MetaDescription = p.PageSummary,
                    CreatedOn = p.CreatedOn
                })
                .ToListAsync();

            return new SearchResult
            {
                Articles = articles,
                Pages = pages,
                CurrentPage = page,
                PageSize = pageSize
            };
        }
    }
}
