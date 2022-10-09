using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    public partial class MainBuildHotfix6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MissonsCreateDate",
                table: "Tasks",
                newName: "MissonsColor");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MissonsColor",
                table: "Tasks",
                newName: "MissonsCreateDate");
        }
    }
}
