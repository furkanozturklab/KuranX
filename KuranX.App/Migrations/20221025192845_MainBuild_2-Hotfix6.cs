using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    public partial class MainBuild_2Hotfix6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResultFinallyNote",
                table: "Results",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 10, 25, 22, 28, 45, 542, DateTimeKind.Local).AddTicks(6452),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 10, 22, 17, 48, 56, 5, DateTimeKind.Local).AddTicks(2141));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResultFinallyNote",
                table: "Results");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 10, 22, 17, 48, 56, 5, DateTimeKind.Local).AddTicks(2141),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 10, 25, 22, 28, 45, 542, DateTimeKind.Local).AddTicks(6452));
        }
    }
}
