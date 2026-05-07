using System.ComponentModel.DataAnnotations;

namespace MyCMS.Core.Entities
{
    public class ArticleTag : BaseEntity
    {
        [Required]
        [MaxLength(500)]
        public string TagName { get; set; }

        [MaxLength(1000)]
        public string TagDesc { get; set; }

        public string TagImage { get; set; }
        public string Slug { get; set; }
    }
}
