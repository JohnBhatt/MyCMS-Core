using System.ComponentModel.DataAnnotations;

namespace MyCMS.Core.Entities
{
    public class MenuItem : BaseEntity
    {
        [Required]
        public Guid MenuId { get; set; }

        [Required]
        [MaxLength(100)]
        public string ItemName { get; set; }

        [Required]
        [MaxLength(500)]
        public string MenuURL { get; set; }

        [MaxLength(500)]
        public string MenuRemarks { get; set; }

        public Guid? ParentMenuItem { get; set; }
        public int ItemPosition { get; set; }
        public bool Visibility { get; set; } = true;
    }
}
