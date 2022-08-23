using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    public partial class TestV6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Integrity",
                columns: table => new
                {
                    IntegrityId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IntergrityName = table.Column<string>(type: "TEXT", nullable: true),
                    connectVerseId = table.Column<int>(type: "INTEGER", nullable: true),
                    connectSureId = table.Column<int>(type: "INTEGER", nullable: true),
                    connectedVerseId = table.Column<int>(type: "INTEGER", nullable: false),
                    connectedSureId = table.Column<int>(type: "INTEGER", nullable: false),
                    InterityNote = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modify = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Integrity", x => x.IntegrityId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Integrity");
        }
    }
}
