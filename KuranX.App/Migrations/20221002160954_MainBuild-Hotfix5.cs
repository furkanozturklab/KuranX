using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    public partial class MainBuildHotfix5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    TasksId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MissonsId = table.Column<int>(type: "INTEGER", nullable: false),
                    MissonsTime = table.Column<int>(type: "INTEGER", nullable: false),
                    MissonsRepeart = table.Column<int>(type: "INTEGER", nullable: false),
                    MissonsType = table.Column<string>(type: "TEXT", nullable: false),
                    MissonsCreateDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.TasksId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
