namespace MyCMS.Core.Entities
{
    public class ThemeConfiguration : BaseEntity
    {
        public Guid ThemeId { get; set; }
        public Theme? Theme { get; set; }
        public string Key { get; set; } = string.Empty; // e.g., "homepage_categories", "sidebar_widgets"
        public string? Value { get; set; } // JSON value for configuration
    }
}
