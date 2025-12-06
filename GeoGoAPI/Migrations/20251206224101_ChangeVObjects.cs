using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeoGoAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeVObjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModelUrlTexture",
                table: "VirtualObjects",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModelUrlTexture",
                table: "VirtualObjects");
        }
    }
}
