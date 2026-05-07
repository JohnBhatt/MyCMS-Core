using MyCMS.Core.Entities;

namespace MyCMS.Core.Interfaces
{
    public interface IPageService
    {
        Task<List<Page>> GetAllPagesAsync();
        Task<Page?> GetPageByIdAsync(Guid id);
        Task<Page?> GetPageByUrlAsync(string url);
        Task<Page> CreatePageAsync(Page page);
        Task<Page> UpdatePageAsync(Page page);
        Task DeletePageAsync(Guid id);
        Task<Page?> SetHomePageAsync(Guid id);
    }
}
