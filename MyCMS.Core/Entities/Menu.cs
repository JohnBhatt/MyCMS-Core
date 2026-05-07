using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MyCMS.Core.Entities
{
    public class Menu : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string MenuName { get; set; }

        [Required]
        [MaxLength(50)]
        public string Position { get; set; } // Main, LeftSideBar, RightSideBar, Footer, TopNavigation

        [MaxLength(200)]
        public string MenuDesc { get; set; }

        public List<MenuItem> MenuItems { get; set; } = new();
    }
}
