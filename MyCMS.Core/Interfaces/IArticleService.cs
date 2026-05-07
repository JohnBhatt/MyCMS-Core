using MyCMS.Core.Entities;

namespace MyCMS.Core.Interfaces
{
    public interface IArticleService
    {
        Task<List<Article>> GetAllArticlesAsync();
        Task<List<Article>> GetPublishedArticlesAsync();
        Task<List<Article>> GetArticlesByCategoryAsync(Guid categoryId);
        Task<List<Article>> GetArticlesByTagAsync(Guid tagId);
        Task<List<Article>> GetFeaturedArticlesAsync();
        Task<Article?> GetArticleByIdAsync(Guid id);
        Task<Article?> GetArticleBySlugAsync(string slug);
        Task<Article> CreateArticleAsync(Article article);
        Task<Article> UpdateArticleAsync(Article article);
        Task DeleteArticleAsync(Guid id);
        Task IncrementViewCountAsync(Guid id);
    }
}
