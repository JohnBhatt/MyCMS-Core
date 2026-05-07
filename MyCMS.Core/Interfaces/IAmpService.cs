namespace MyCMS.Core.Interfaces
{
    public interface IAmpService
    {
        Task<string?> GenerateAmpPageAsync(Guid articleId);
        Task<bool> ValidateAmpPageAsync(string ampHtml);
    }
}
