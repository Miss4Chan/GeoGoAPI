using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeoGoAPI.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "BLOB", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeletedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Places", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Places_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserTwins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTwins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTwins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VirtualPlaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlaceId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VirtualPlaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VirtualPlaces_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryWeights",
                columns: table => new
                {
                    UserTwinId = table.Column<int>(type: "INTEGER", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    Score = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryWeights", x => new { x.UserTwinId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_CategoryWeights_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryWeights_UserTwins_UserTwinId",
                        column: x => x.UserTwinId,
                        principalTable: "UserTwins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlaceLikes",
                columns: table => new
                {
                    UserTwinId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlaceId = table.Column<int>(type: "INTEGER", nullable: false),
                    Score = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceLikes", x => new { x.UserTwinId, x.PlaceId });
                    table.ForeignKey(
                        name: "FK_PlaceLikes_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaceLikes_UserTwins_UserTwinId",
                        column: x => x.UserTwinId,
                        principalTable: "UserTwins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VirtualObjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VirtualPlaceId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ModelUrl = table.Column<string>(type: "TEXT", nullable: false),
                    PX = table.Column<float>(type: "REAL", nullable: false),
                    PY = table.Column<float>(type: "REAL", nullable: false),
                    PZ = table.Column<float>(type: "REAL", nullable: false),
                    RX = table.Column<float>(type: "REAL", nullable: false),
                    RY = table.Column<float>(type: "REAL", nullable: false),
                    RZ = table.Column<float>(type: "REAL", nullable: false),
                    SX = table.Column<float>(type: "REAL", nullable: false),
                    SY = table.Column<float>(type: "REAL", nullable: false),
                    SZ = table.Column<float>(type: "REAL", nullable: false),
                    TextDisplayed = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VirtualObjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VirtualObjects_VirtualPlaces_VirtualPlaceId",
                        column: x => x.VirtualPlaceId,
                        principalTable: "VirtualPlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InteractionEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserTwinId = table.Column<int>(type: "INTEGER", nullable: true),
                    PlaceId = table.Column<int>(type: "INTEGER", nullable: true),
                    VirtualPlaceId = table.Column<int>(type: "INTEGER", nullable: true),
                    VirtualObjectId = table.Column<int>(type: "INTEGER", nullable: true),
                    VirtualAnimationId = table.Column<int>(type: "INTEGER", nullable: true),
                    EventType = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Metadata = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InteractionEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InteractionEvents_PlaceLikes_UserTwinId_PlaceId",
                        columns: x => new { x.UserTwinId, x.PlaceId },
                        principalTable: "PlaceLikes",
                        principalColumns: new[] { "UserTwinId", "PlaceId" },
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_InteractionEvents_UserTwins_UserTwinId",
                        column: x => x.UserTwinId,
                        principalTable: "UserTwins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InteractionEvents_VirtualObjects_VirtualObjectId",
                        column: x => x.VirtualObjectId,
                        principalTable: "VirtualObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_InteractionEvents_VirtualPlaces_VirtualPlaceId",
                        column: x => x.VirtualPlaceId,
                        principalTable: "VirtualPlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryWeights_CategoryId",
                table: "CategoryWeights",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_InteractionEvents_UserTwinId_PlaceId",
                table: "InteractionEvents",
                columns: new[] { "UserTwinId", "PlaceId" });

            migrationBuilder.CreateIndex(
                name: "IX_InteractionEvents_VirtualObjectId",
                table: "InteractionEvents",
                column: "VirtualObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_InteractionEvents_VirtualPlaceId",
                table: "InteractionEvents",
                column: "VirtualPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaceLikes_PlaceId",
                table: "PlaceLikes",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Places_CategoryId",
                table: "Places",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTwins_UserId",
                table: "UserTwins",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VirtualObjects_VirtualPlaceId",
                table: "VirtualObjects",
                column: "VirtualPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_VirtualPlaces_PlaceId",
                table: "VirtualPlaces",
                column: "PlaceId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryWeights");

            migrationBuilder.DropTable(
                name: "InteractionEvents");

            migrationBuilder.DropTable(
                name: "PlaceLikes");

            migrationBuilder.DropTable(
                name: "VirtualObjects");

            migrationBuilder.DropTable(
                name: "UserTwins");

            migrationBuilder.DropTable(
                name: "VirtualPlaces");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Places");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
