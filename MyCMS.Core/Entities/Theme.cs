namespace MyCMS.Core.Entities
{
    public class Theme : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string FolderName { get; set; } = string.Empty;
        public string? Thumbnail { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = false;
        public string? CustomCss { get; set; }
        public string? ColorScheme { get; set; } // JSON for color customization
        public string? LayoutOptions { get; set; } // JSON for layout options (sidebar position, etc.)
        public bool IsDefault { get; set; } = false;
    }
}
