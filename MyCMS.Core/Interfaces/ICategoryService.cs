using MyCMS.Core.Entities;

namespace MyCMS.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<List<ArticleCategory>> GetAllCategoriesAsync();
        Task<ArticleCategory?> GetCategoryByIdAsync(Guid id);
        Task<ArticleCategory> CreateCategoryAsync(ArticleCategory category);
        Task<ArticleCategory> UpdateCategoryAsync(ArticleCategory category);
        Task DeleteCategoryAsync(Guid id);
    }
}
