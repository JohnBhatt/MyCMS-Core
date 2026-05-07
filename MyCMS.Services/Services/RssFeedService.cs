using System.Xml;
using MyCMS.Core.Interfaces;
using MyCMS.Data;

namespace MyCMS.Services
{
    public class RssFeedService : IRssFeedService
    {
        private readonly AppDbContext _context;
        private readonly IConfigurationService _configService;

        public RssFeedService(AppDbContext context, IConfigurationService configService)
        {
            _context = context;
            _configService = configService;
        }

        public async Task<string> GenerateRssFeedAsync()
        {
            var config = await _configService.GetConfigurationAsync();
            var baseUrl = config?.BaseURL ?? "https://localhost";
            var feedTitle = config?.RSSFeedTitle ?? "MyCMS Articles";
            var feedDescription = config?.RSSFeedDescription ?? "Latest articles from MyCMS";
            var itemCount = config?.RSSFeedItemCount ?? 20;

            var articles = await _context.Articles
                .Where(a => a.IsPublished)
                .OrderByDescending(a => a.PublishedDate)
                .Take(itemCount)
                .ToListAsync();

            return GenerateRssXml(articles, baseUrl, feedTitle, feedDescription);
        }

        public async Task<string> GenerateCategoryRssFeedAsync(Guid categoryId)
        {
            var config = await _configService.GetConfigurationAsync();
            var baseUrl = config?.BaseURL ?? "https://localhost";

            var articles = await _context.Articles
                .Where(a => a.CategoryId == categoryId && a.IsPublished)
                .OrderByDescending(a => a.PublishedDate)
                .ToListAsync();

            return GenerateRssXml(articles, baseUrl, "Category Feed", "Articles by category");
        }

        public async Task<string> GenerateTagRssFeedAsync(Guid tagId)
        {
            var config = await _configService.GetConfigurationAsync();
            var baseUrl = config?.BaseURL ?? "https://localhost";

            var articles = await _context.Articles
                .Where(a => a.ArticleTagMappings.Any(atm => atm.TagId == tagId) && a.IsPublished)
                .OrderByDescending(a => a.PublishedDate)
                .ToListAsync();

            return GenerateRssXml(articles, baseUrl, "Tag Feed", "Articles by tag");
        }

        private string GenerateRssXml(List<Core.Entities.Article> articles, string baseUrl, string title, string description)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = System.Text.Encoding.UTF8
            };

            using var stringWriter = new System.IO.StringWriter();
            using var writer = XmlWriter.Create(stringWriter, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("rss");
            writer.WriteAttributeString("version", "2.0");

            writer.WriteStartElement("channel");
            writer.WriteElementString("title", title);
            writer.WriteElementString("description", description);
            writer.WriteElementString("link", baseUrl);
            writer.WriteElementString("lastBuildDate", DateTime.UtcNow.ToString("R"));

            foreach (var article in articles)
            {
                writer.WriteStartElement("item");
                writer.WriteElementString("title", article.Title);
                writer.WriteElementString("description", article.MetaDescription ?? "");
                writer.WriteElementString("link", $"{baseUrl}/articles/{article.Slug}");
                writer.WriteElementString("pubDate", article.PublishedDate?.ToString("R") ?? article.CreatedOn.ToString("R"));
                writer.WriteElementString("guid", article.Id.ToString());
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();

            return stringWriter.ToString();
        }
    }
}
