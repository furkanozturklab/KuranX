using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    public partial class Testv1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sure",
                columns: table => new
                {
                    sureId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    NumberOfVerses = table.Column<int>(type: "INTEGER", nullable: true),
                    LandingLocation = table.Column<string>(type: "TEXT", nullable: true),
                    DeskLanding = table.Column<int>(type: "INTEGER", nullable: true),
                    DeskMushaf = table.Column<int>(type: "INTEGER", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sure", x => x.sureId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user_email = table.Column<string>(type: "TEXT", nullable: false),
                    user_fisrtName = table.Column<string>(type: "TEXT", nullable: true, defaultValue: "First Name"),
                    user_lastName = table.Column<string>(type: "TEXT", nullable: true, defaultValue: "Last Name"),
                    user_password = table.Column<string>(type: "TEXT", nullable: true),
                    user_screetQuestion = table.Column<string>(type: "TEXT", nullable: true, defaultValue: "Screet Question"),
                    user_createDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    user_updateDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    user_avatarUrl = table.Column<string>(type: "TEXT", nullable: true, defaultValue: "default")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "Verse",
                columns: table => new
                {
                    VerseId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SureId = table.Column<int>(type: "INTEGER", nullable: true),
                    RelativeDesk = table.Column<int>(type: "INTEGER", nullable: true),
                    VerseDesk = table.Column<int>(type: "INTEGER", nullable: true),
                    VerseArabic = table.Column<string>(type: "TEXT", nullable: true),
                    VerseTr = table.Column<string>(type: "TEXT", nullable: true),
                    VerseDesc = table.Column<string>(type: "TEXT", nullable: true),
                    VerseCheck = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verse", x => x.VerseId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sure");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Verse");
        }
    }
}
