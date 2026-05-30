using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCMS.Data.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class FixMenuItemRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_Menus_MenuId1",
                table: "MenuItems");

            migrationBuilder.DropIndex(
                name: "IX_MenuItems_MenuId1",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "MenuId1",
                table: "MenuItems");

            migrationBuilder.UpdateData(
                table: "Themes",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "Thumbnail",
                value: "/Themes/Minimal/assets/thumbnail.svg");

            migrationBuilder.UpdateData(
                table: "Themes",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "Thumbnail",
                value: "/Themes/Blog/assets/thumbnail.svg");

            migrationBuilder.UpdateData(
                table: "Themes",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "Thumbnail",
                value: "/Themes/Magazine/assets/thumbnail.svg");

            migrationBuilder.UpdateData(
                table: "Themes",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "Thumbnail",
                value: "/Themes/Modern/assets/thumbnail.svg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MenuId1",
                table: "MenuItems",
                type: "uuid",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Themes",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "Thumbnail",
                value: "/Themes/Minimal/assets/thumbnail.png");

            migrationBuilder.UpdateData(
                table: "Themes",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "Thumbnail",
                value: "/Themes/Blog/assets/thumbnail.png");

            migrationBuilder.UpdateData(
                table: "Themes",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "Thumbnail",
                value: "/Themes/Magazine/assets/thumbnail.png");

            migrationBuilder.UpdateData(
                table: "Themes",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "Thumbnail",
                value: "/Themes/Modern/assets/thumbnail.png");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_MenuId1",
                table: "MenuItems",
                column: "MenuId1");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_Menus_MenuId1",
                table: "MenuItems",
                column: "MenuId1",
                principalTable: "Menus",
                principalColumn: "Id");
        }
    }
}
