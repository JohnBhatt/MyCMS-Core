using System.ComponentModel.DataAnnotations;

namespace MyCMS.Core.Entities
{
    public class Upload : BaseEntity
    {
        [Required]
        [MaxLength(400)]
        public string DocumentName { get; set; }

        [Required]
        [MaxLength(200)]
        public string DocumentType { get; set; }

        [MaxLength(500)]
        public string UniqueName { get; set; }

        [Required]
        [MaxLength(1000)]
        public string FilePath { get; set; }

        [MaxLength(1000)]
        public string FileDesc { get; set; }

        public long ViewCount { get; set; } = 0;
    }
}
