using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCMS.Data.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class AddThemeSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleTagMappings_Articles_ArticleId1",
                table: "ArticleTagMappings");

            migrationBuilder.DropIndex(
                name: "IX_ArticleTagMappings_ArticleId1",
                table: "ArticleTagMappings");

            migrationBuilder.DropColumn(
                name: "ArticleId1",
                table: "ArticleTagMappings");

            migrationBuilder.CreateTable(
                name: "Themes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    FolderName = table.Column<string>(type: "text", nullable: false),
                    Thumbnail = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CustomCss = table.Column<string>(type: "text", nullable: true),
                    ColorScheme = table.Column<string>(type: "text", nullable: true),
                    LayoutOptions = table.Column<string>(type: "text", nullable: true),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Themes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThemeConfigurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ThemeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ThemeId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    Key = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThemeConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThemeConfigurations_Themes_ThemeId",
                        column: x => x.ThemeId,
                        principalTable: "Themes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ThemeConfigurations_Themes_ThemeId1",
                        column: x => x.ThemeId1,
                        principalTable: "Themes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ThemeConfigurations_ThemeId",
                table: "ThemeConfigurations",
                column: "ThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_ThemeConfigurations_ThemeId1",
                table: "ThemeConfigurations",
                column: "ThemeId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThemeConfigurations");

            migrationBuilder.DropTable(
                name: "Themes");

            migrationBuilder.AddColumn<Guid>(
                name: "ArticleId1",
                table: "ArticleTagMappings",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTagMappings_ArticleId1",
                table: "ArticleTagMappings",
                column: "ArticleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleTagMappings_Articles_ArticleId1",
                table: "ArticleTagMappings",
                column: "ArticleId1",
                principalTable: "Articles",
                principalColumn: "Id");
        }
    }
}
