using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    public partial class TestV4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Interpreter",
                columns: table => new
                {
                    interpreterId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    versesId = table.Column<int>(type: "INTEGER", nullable: true),
                    sureId = table.Column<int>(type: "INTEGER", nullable: true),
                    interpreterWriter = table.Column<string>(type: "TEXT", nullable: true),
                    interpreterDetail = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interpreter", x => x.interpreterId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Interpreter");
        }
    }
}
