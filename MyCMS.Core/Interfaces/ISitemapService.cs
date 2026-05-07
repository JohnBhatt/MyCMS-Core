namespace MyCMS.Core.Interfaces
{
    public interface ISitemapService
    {
        Task<string> GenerateSitemapAsync();
    }
}
