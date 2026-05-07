using Microsoft.EntityFrameworkCore;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;
using MyCMS.Data;

namespace MyCMS.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly AppDbContext _context;

        public ConfigurationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Configuration?> GetConfigurationAsync(string sectionName = "Default")
        {
            return await _context.Configurations
                .FirstOrDefaultAsync(c => c.SectionName == sectionName);
        }

        public async Task<Configuration> UpdateConfigurationAsync(Configuration configuration)
        {
            var existing = await _context.Configurations
                .FirstOrDefaultAsync(c => c.SectionName == configuration.SectionName);

            if (existing != null)
            {
                existing.SiteName = configuration.SiteName;
                existing.Slogan = configuration.Slogan;
                existing.BaseURL = configuration.BaseURL;
                existing.FacebookLink = configuration.FacebookLink;
                existing.GooglePlusLink = configuration.GooglePlusLink;
                existing.LinkedInLink = configuration.LinkedInLink;
                existing.TwitterLink = configuration.TwitterLink;
                existing.MasterPageName = configuration.MasterPageName;
                existing.FooterText = configuration.FooterText;
                existing.Keywords = configuration.Keywords;
                existing.MetaDesc = configuration.MetaDesc;
                existing.ThemeName = configuration.ThemeName;
                existing.SitemapEnabled = configuration.SitemapEnabled;
                existing.SitemapPriority = configuration.SitemapPriority;
                existing.SitemapChangeFrequency = configuration.SitemapChangeFrequency;
                existing.RSSFeedEnabled = configuration.RSSFeedEnabled;
                existing.RSSFeedTitle = configuration.RSSFeedTitle;
                existing.RSSFeedDescription = configuration.RSSFeedDescription;
                existing.RSSFeedItemCount = configuration.RSSFeedItemCount;
                existing.HeadScript = configuration.HeadScript;
                existing.BodyScript = configuration.BodyScript;
                existing.GoogleAnalyticsId = configuration.GoogleAnalyticsId;
                existing.GoogleAdsenseId = configuration.GoogleAdsenseId;
                existing.FacebookPixelId = configuration.FacebookPixelId;
                existing.AMPEnabled = configuration.AMPEnabled;
                existing.AMPAnalyticsId = configuration.AMPAnalyticsId;
                existing.AMPLogoUrl = configuration.AMPLogoUrl;
                existing.AMPublisherName = configuration.AMPublisherName;
                existing.ModifiedOn = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return existing;
            }
            else
            {
                configuration.Id = Guid.NewGuid();
                configuration.CreatedOn = DateTime.UtcNow;
                _context.Configurations.Add(configuration);
                await _context.SaveChangesAsync();
                return configuration;
            }
        }
    }
}
