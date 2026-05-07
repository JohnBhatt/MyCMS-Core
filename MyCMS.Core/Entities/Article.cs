using System;
using System.ComponentModel.DataAnnotations;

namespace MyCMS.Core.Entities
{
    public class Article : BaseEntity
    {
        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Title { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Slug { get; set; }

        [MaxLength(1000)]
        public string MetaDescription { get; set; }

        [MaxLength(2000)]
        public string MetaKeywords { get; set; }

        public string FeaturedImage { get; set; }
        public string Content { get; set; }
        public string Tags { get; set; }
        public long ViewCount { get; set; } = 0;
        public bool IsFeatured { get; set; } = false;
        public DateTime? FeaturedFrom { get; set; }
        public DateTime? FeaturedUpto { get; set; }
        public bool IsPublished { get; set; } = false;
        public DateTime? PublishedDate { get; set; }
    }
}
