using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    public partial class MainBuild_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResultItems",
                columns: table => new
                {
                    ResultItemId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ResultId = table.Column<int>(type: "INTEGER", nullable: false),
                    ResultNoteId = table.Column<int>(type: "INTEGER", nullable: false),
                    ResultLibId = table.Column<int>(type: "INTEGER", nullable: false),
                    ResultSubjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    ResultVerseId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultItems", x => x.ResultItemId);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    ResultId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ResultName = table.Column<string>(type: "TEXT", nullable: false),
                    ResultVerse = table.Column<string>(type: "TEXT", nullable: false),
                    ResultLib = table.Column<string>(type: "TEXT", nullable: false),
                    ResultNotes = table.Column<string>(type: "TEXT", nullable: false),
                    ResultSubject = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.ResultId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResultItems");

            migrationBuilder.DropTable(
                name: "Results");
        }
    }
}