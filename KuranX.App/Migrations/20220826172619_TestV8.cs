using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    public partial class TestV8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    WordsId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VerseId = table.Column<int>(type: "INTEGER", nullable: true),
                    SureId = table.Column<int>(type: "INTEGER", nullable: true),
                    WordText = table.Column<string>(type: "TEXT", nullable: true),
                    WordRe = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.WordsId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Words");
        }
    }
}
