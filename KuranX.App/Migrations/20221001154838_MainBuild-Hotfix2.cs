using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    public partial class MainBuildHotfix2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Create",
                table: "Remider",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LoopType",
                table: "Remider",
                type: "TEXT",
                nullable: false,
                defaultValue: "False");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Remider",
                type: "TEXT",
                nullable: false,
                defaultValue: "Wait");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Create",
                table: "Remider");

            migrationBuilder.DropColumn(
                name: "LoopType",
                table: "Remider");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Remider");
        }
    }
}
