using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodePulse_API.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class AddedRefreshToken1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityUser");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiryTime", "SecurityStamp", "Token", "TwoFactorEnabled", "UserName" },
                values: new object[] { "f3ead830 - a200 - 4b5b - 9997 - 58a424f7dcef", 0, "ddf7a5b9-f91d-465a-a1e3-29a24dc6d452", "admin@codePulse.com", false, false, null, "ADMIN@CODEPULSE.COM", "ADMIN@CODEPULSE.COM", "AQAAAAIAAYagAAAAEDl+MRs9SSkg+WrsfytzsubUlMW89UO5wPozaDf2SEPvbkhFekGzdhxMwe1OlSEibQ==", null, false, "", new DateTime(2024, 8, 30, 11, 27, 59, 850, DateTimeKind.Local).AddTicks(5101), "29287299-523e-41e5-aa0f-ae80788c4270", "", false, "admin@codePulse.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f3ead830 - a200 - 4b5b - 9997 - 58a424f7dcef");

            migrationBuilder.CreateTable(
                name: "IdentityUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUser", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "IdentityUser",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "f3ead830 - a200 - 4b5b - 9997 - 58a424f7dcef", 0, "6384820a-5775-4993-9926-cd3ed8662ff3", "admin@codePulse.com", false, false, null, "ADMIN@CODEPULSE.COM", "ADMIN@CODEPULSE.COM", "AQAAAAIAAYagAAAAEGmVi5a3zy9Q7TDS0JeTz+MYawKjyXpmn5osTx5/S7byFgdhS6lEYZBfOdx5U4cG7w==", null, false, "f10c7a4e-ec02-42c4-b4f0-3ef5f93620d9", false, "admin@codePulse.com" });
        }
    }
}
