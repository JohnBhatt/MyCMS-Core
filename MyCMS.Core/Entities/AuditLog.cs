using System.ComponentModel.DataAnnotations;

namespace MyCMS.Core.Entities
{
    public class AuditLog : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string TableName { get; set; } // e.g., "Settings", "Articles", "Users"

        [Required]
        [MaxLength(100)]
        public string RecordId { get; set; } // GUID as string for flexibility

        [Required]
        [MaxLength(50)]
        public string Action { get; set; } // Created, Updated, Deleted

        public string OldValues { get; set; } // JSON serialized old values

        public string NewValues { get; set; } // JSON serialized new values

        [MaxLength(500)]
        public string ChangeReason { get; set; }

        public Guid? ModifiedBy { get; set; }

        [MaxLength(200)]
        public string ModifiedByUserName { get; set; }

        [MaxLength(50)]
        public string IpAddress { get; set; }

        [MaxLength(500)]
        public string UserAgent { get; set; }
    }
}
