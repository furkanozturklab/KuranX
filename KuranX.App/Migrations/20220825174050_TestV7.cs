using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    public partial class TestV7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    SubjectId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SubjectName = table.Column<string>(type: "TEXT", nullable: true),
                    SubjectColor = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modify = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.SubjectId);
                });

            migrationBuilder.CreateTable(
                name: "SubjectItems",
                columns: table => new
                {
                    SubjectItemsId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SureId = table.Column<int>(type: "INTEGER", nullable: true),
                    VerseId = table.Column<int>(type: "INTEGER", nullable: true),
                    SubjectNotesId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectItems", x => x.SubjectItemsId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subject");

            migrationBuilder.DropTable(
                name: "SubjectItems");
        }
    }
}
