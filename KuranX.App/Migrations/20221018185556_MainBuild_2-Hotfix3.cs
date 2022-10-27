using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    public partial class MainBuild_2Hotfix3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoteLibHeader",
                table: "Notes");

            migrationBuilder.AddColumn<int>(
                name: "LibraryId",
                table: "Notes",
                type: "INTEGER",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Librarys",
                columns: table => new
                {
                    LibraryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LibraryName = table.Column<string>(type: "TEXT", nullable: true),
                    LibraryColor = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modify = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Librarys", x => x.LibraryId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Librarys");

            migrationBuilder.DropColumn(
                name: "LibraryId",
                table: "Notes");

            migrationBuilder.AddColumn<string>(
                name: "NoteLibHeader",
                table: "Notes",
                type: "TEXT",
                nullable: true,
                defaultValue: "Default");
        }
    }
}
