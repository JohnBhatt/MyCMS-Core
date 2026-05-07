using System.Xml;
using MyCMS.Core.Interfaces;
using MyCMS.Data;

namespace MyCMS.Services
{
    public class SitemapService : ISitemapService
    {
        private readonly AppDbContext _context;
        private readonly IConfigurationService _configService;

        public SitemapService(AppDbContext context, IConfigurationService configService)
        {
            _context = context;
            _configService = configService;
        }

        public async Task<string> GenerateSitemapAsync()
        {
            var config = await _configService.GetConfigurationAsync();
            var baseUrl = config?.BaseURL ?? "https://localhost";
            var defaultPriority = config?.SitemapPriority ?? 0.5m;
            var changeFrequency = config?.SitemapChangeFrequency ?? "weekly";

            var settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = System.Text.Encoding.UTF8
            };

            using var stringWriter = new System.IO.StringWriter();
            using var writer = XmlWriter.Create(stringWriter, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

            // Add pages
            var pages = await _context.Pages.ToListAsync();
            foreach (var page in pages)
            {
                writer.WriteStartElement("url");
                writer.WriteElementString("loc", $"{baseUrl}/page/{page.PageURL}");
                writer.WriteElementString("lastmod", page.ModifiedOn?.ToString("yyyy-MM-dd") ?? page.CreatedOn.ToString("yyyy-MM-dd"));
                writer.WriteElementString("changefreq", changeFrequency);
                writer.WriteElementString("priority", defaultPriority.ToString("F1"));
                writer.WriteEndElement();
            }

            // Add articles
            var articles = await _context.Articles.Where(a => a.IsPublished).ToListAsync();
            foreach (var article in articles)
            {
                writer.WriteStartElement("url");
                writer.WriteElementString("loc", $"{baseUrl}/articles/{article.Slug}");
                writer.WriteElementString("lastmod", article.ModifiedOn?.ToString("yyyy-MM-dd") ?? article.CreatedOn.ToString("yyyy-MM-dd"));
                writer.WriteElementString("changefreq", changeFrequency);
                writer.WriteElementString("priority", defaultPriority.ToString("F1"));
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();

            return stringWriter.ToString();
        }
    }
}
