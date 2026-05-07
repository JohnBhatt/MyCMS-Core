using System;
using System.ComponentModel.DataAnnotations;

namespace MyCMS.Core.Entities
{
    public class Page : BaseEntity
    {
        [Required]
        [MaxLength(300)]
        public string PageTitle { get; set; }

        [Required]
        [MaxLength(300)]
        public string PageURL { get; set; }

        public Guid? ParentPage { get; set; }
        public string PageSummary { get; set; }
        public string PageBody { get; set; }
        public long ViewCount { get; set; } = 0;
        public bool IsHomePage { get; set; } = false;
    }
}
