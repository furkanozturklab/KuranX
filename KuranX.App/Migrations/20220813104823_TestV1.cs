using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    public partial class TestV1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user_email = table.Column<string>(type: "TEXT", nullable: false),
                    user_fisrtName = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "First Name"),
                    user_lastName = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "Last Name"),
                    user_password = table.Column<string>(type: "TEXT", nullable: false),
                    user_screetQuestion = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "Screet Question"),
                    user_createDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    user_updateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    user_avatarUrl = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "default")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "Verses",
                columns: table => new
                {
                    versesId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    NumberOfVerses = table.Column<int>(type: "INTEGER", nullable: false),
                    LandingLocation = table.Column<string>(type: "TEXT", nullable: false),
                    DeskLanding = table.Column<int>(type: "INTEGER", nullable: false),
                    DeskMushaf = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verses", x => x.versesId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Verses");
        }
    }
}
