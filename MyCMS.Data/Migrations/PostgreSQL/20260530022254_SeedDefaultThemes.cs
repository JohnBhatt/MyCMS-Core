using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyCMS.Data.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class SeedDefaultThemes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Themes",
                columns: new[] { "Id", "ColorScheme", "CreatedBy", "CreatedOn", "CustomCss", "Description", "DisplayName", "FolderName", "IsActive", "IsDefault", "IsDeleted", "LayoutOptions", "ModifiedBy", "ModifiedOn", "Name", "Thumbnail" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Clean, typography-focused design with elegant serif headings and lots of whitespace.", "Minimal", "Minimal", true, true, false, null, null, null, "Minimal", "/Themes/Minimal/assets/thumbnail.png" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Classic blog layout with sidebar, blue accent, and card-based article grid.", "Blog", "Blog", false, false, false, null, null, null, "Blog", "/Themes/Blog/assets/thumbnail.png" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bold magazine style with dark hero section, featured posts, and trending sidebar.", "Magazine", "Magazine", false, false, false, null, null, null, "Magazine", "/Themes/Magazine/assets/thumbnail.png" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Vibrant gradients (purple/pink), rounded cards, and configurable category grid with icons.", "Modern", "Modern", false, false, false, null, null, null, "Modern", "/Themes/Modern/assets/thumbnail.png" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Themes",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Themes",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Themes",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Themes",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));
        }
    }
}
