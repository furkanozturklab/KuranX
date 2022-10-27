using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    public partial class MainBuild_2Hotfix7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ResultFinallyNote",
                table: "Results",
                type: "TEXT",
                nullable: false,
                defaultValue: "Sonuç Metninizi buraya yaza bilirsiniz.",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 10, 27, 19, 9, 49, 542, DateTimeKind.Local).AddTicks(4888),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 10, 25, 22, 28, 45, 542, DateTimeKind.Local).AddTicks(6452));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ResultFinallyNote",
                table: "Results",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "Sonuç Metninizi buraya yaza bilirsiniz.");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 10, 25, 22, 28, 45, 542, DateTimeKind.Local).AddTicks(6452),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 10, 27, 19, 9, 49, 542, DateTimeKind.Local).AddTicks(4888));
        }
    }
}
