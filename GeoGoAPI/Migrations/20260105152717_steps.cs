using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeoGoAPI.Migrations
{
    /// <inheritdoc />
    public partial class steps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StepsJson",
                table: "VirtualObjects",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StepsJson",
                table: "VirtualObjects");
        }
    }
}
