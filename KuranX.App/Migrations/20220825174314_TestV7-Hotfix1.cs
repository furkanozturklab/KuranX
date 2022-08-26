using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    public partial class TestV7Hotfix1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "SubjectItems",
                type: "INTEGER",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "SubjectItems");
        }
    }
}
