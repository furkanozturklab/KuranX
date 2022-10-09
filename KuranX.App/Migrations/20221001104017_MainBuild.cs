using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    public partial class MainBuild : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Remider",
                columns: table => new
                {
                    RemiderId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RemiderName = table.Column<string>(type: "TEXT", nullable: false),
                    RemiderDetail = table.Column<string>(type: "TEXT", nullable: false),
                    RemiderDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ConnectVerseId = table.Column<int>(type: "INTEGER", nullable: false),
                    ConnectSureId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remider", x => x.RemiderId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Remider");
        }
    }
}
