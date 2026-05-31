using System.ComponentModel.DataAnnotations;

namespace MyCMS.Core.Entities
{
    public class Setting : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Key { get; set; }

        public string Value { get; set; }

        [MaxLength(50)]
        public string Category { get; set; } // Social, SEO, General, Theme, etc.

        [MaxLength(500)]
        public string Description { get; set; }

        public bool IsEncrypted { get; set; } = false;

        public bool IsEditable { get; set; } = true;
    }
}
