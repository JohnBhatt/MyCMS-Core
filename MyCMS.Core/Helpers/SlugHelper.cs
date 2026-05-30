using System.Text;
using System.Text.RegularExpressions;

namespace MyCMS.Core.Helpers
{
    public static class SlugHelper
    {
        public static string GenerateSlug(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // Convert to lowercase
            string slug = input.ToLowerInvariant();

            // Remove special characters except spaces and hyphens
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");

            // Replace spaces with hyphens
            slug = Regex.Replace(slug, @"\s+", "-");

            // Remove consecutive hyphens
            slug = Regex.Replace(slug, @"-+", "-");

            // Trim hyphens from start and end
            slug = slug.Trim('-');

            // Limit length to 200 characters
            if (slug.Length > 200)
                slug = slug.Substring(0, 200).Trim('-');

            return slug;
        }

        public static string SanitizeSlug(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return string.Empty;

            // Convert to lowercase
            slug = slug.ToLowerInvariant();

            // Remove special characters except spaces and hyphens
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");

            // Replace spaces with hyphens
            slug = Regex.Replace(slug, @"\s+", "-");

            // Remove consecutive hyphens
            slug = Regex.Replace(slug, @"-+", "-");

            // Trim hyphens from start and end
            slug = slug.Trim('-');

            // Limit length to 200 characters
            if (slug.Length > 200)
                slug = slug.Substring(0, 200).Trim('-');

            return slug;
        }
    }
}
