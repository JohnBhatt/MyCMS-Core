using System.ComponentModel.DataAnnotations;

namespace MyCMS.Core.Entities
{
    public class OpenGraphTag : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string TagName { get; set; }

        [MaxLength(200)]
        public string TagProperty { get; set; }

        public string TagContent { get; set; }
        public bool Visibility { get; set; } = true;
    }
}
