using System.ComponentModel.DataAnnotations;

namespace MyCMS.Core.Entities
{
    public class ArticleCategory : BaseEntity
    {
        [Required]
        [MaxLength(500)]
        public string CategoryName { get; set; }

        public string CatDesc { get; set; }
        public Guid? ParentCategory { get; set; }
        public string? CategoryImage { get; set; }
        public string Slug { get; set; }
    }
}
