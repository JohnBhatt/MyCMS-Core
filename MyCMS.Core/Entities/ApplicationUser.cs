using Microsoft.AspNetCore.Identity;

namespace MyCMS.Core.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserRole { get; set; }
        public DateTime? UserDOB { get; set; }
        public DateTime RegDate { get; set; } = DateTime.UtcNow;
        public bool IsAuthorized { get; set; } = false;
        public bool IsSuspended { get; set; } = false;
    }
}
