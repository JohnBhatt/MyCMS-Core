using MyCMS.Core.Entities;

namespace MyCMS.Data.Data
{
    public static class ThemeSeedData
    {
        public static List<Theme> GetDefaultThemes()
        {
            var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            
            return new List<Theme>
            {
                new Theme
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "Minimal",
                    DisplayName = "Minimal",
                    FolderName = "Minimal",
                    Thumbnail = "/Themes/Minimal/assets/thumbnail.svg",
                    Description = "Clean, typography-focused design with elegant serif headings and lots of whitespace.",
                    IsActive = true,
                    IsDefault = true,
                    CreatedOn = seedDate
                },
                new Theme
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Blog",
                    DisplayName = "Blog",
                    FolderName = "Blog",
                    Thumbnail = "/Themes/Blog/assets/thumbnail.svg",
                    Description = "Classic blog layout with sidebar, blue accent, and card-based article grid.",
                    IsActive = false,
                    IsDefault = false,
                    CreatedOn = seedDate
                },
                new Theme
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "Magazine",
                    DisplayName = "Magazine",
                    FolderName = "Magazine",
                    Thumbnail = "/Themes/Magazine/assets/thumbnail.svg",
                    Description = "Bold magazine style with dark hero section, featured posts, and trending sidebar.",
                    IsActive = false,
                    IsDefault = false,
                    CreatedOn = seedDate
                },
                new Theme
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Name = "Modern",
                    DisplayName = "Modern",
                    FolderName = "Modern",
                    Thumbnail = "/Themes/Modern/assets/thumbnail.svg",
                    Description = "Vibrant gradients (purple/pink), rounded cards, and configurable category grid with icons.",
                    IsActive = false,
                    IsDefault = false,
                    CreatedOn = seedDate
                }
            };
        }
    }
}
