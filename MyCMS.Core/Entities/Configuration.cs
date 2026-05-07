using System.ComponentModel.DataAnnotations;

namespace MyCMS.Core.Entities
{
    public class Configuration : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string SectionName { get; set; } // Default (Home), Forum, Articles, etc.

        [Required]
        [MaxLength(500)]
        public string SiteName { get; set; }

        [MaxLength(500)]
        public string Slogan { get; set; }

        [MaxLength(400)]
        public string BaseURL { get; set; }

        [MaxLength(200)]
        public string FacebookLink { get; set; }

        [MaxLength(200)]
        public string GooglePlusLink { get; set; }

        [MaxLength(200)]
        public string LinkedInLink { get; set; }

        [MaxLength(200)]
        public string TwitterLink { get; set; }

        [MaxLength(200)]
        public string MasterPageName { get; set; }

        public string FooterText { get; set; }
        public string Keywords { get; set; }
        public string MetaDesc { get; set; }

        public long VisitorCount { get; set; } = 0;

        [MaxLength(100)]
        public string ThemeName { get; set; }

        // Sitemap settings
        public bool SitemapEnabled { get; set; } = true;
        public decimal SitemapPriority { get; set; } = 0.5m;
        public string SitemapChangeFrequency { get; set; } = "weekly";

        // RSS Feed settings
        public bool RSSFeedEnabled { get; set; } = true;
        public string RSSFeedTitle { get; set; }
        public string RSSFeedDescription { get; set; }
        public int RSSFeedItemCount { get; set; } = 20;

        // Custom Scripts
        public string HeadScript { get; set; }
        public string BodyScript { get; set; }
        public string GoogleAnalyticsId { get; set; }
        public string GoogleAdsenseId { get; set; }
        public string FacebookPixelId { get; set; }

        // AMP Settings
        public bool AMPEnabled { get; set; } = true;
        public string AMPAnalyticsId { get; set; }
        public string AMPLogoUrl { get; set; }
        public string AMPublisherName { get; set; }
    }
}
