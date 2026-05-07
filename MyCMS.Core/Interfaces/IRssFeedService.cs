namespace MyCMS.Core.Interfaces
{
    public interface IRssFeedService
    {
        Task<string> GenerateRssFeedAsync();
        Task<string> GenerateCategoryRssFeedAsync(Guid categoryId);
        Task<string> GenerateTagRssFeedAsync(Guid tagId);
    }
}
