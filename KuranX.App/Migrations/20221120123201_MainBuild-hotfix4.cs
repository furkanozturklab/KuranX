using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    /// <inheritdoc />
    public partial class MainBuildhotfix4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PdfPageId",
                table: "Notes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 20, 15, 32, 1, 302, DateTimeKind.Local).AddTicks(3104),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 20, 15, 30, 14, 837, DateTimeKind.Local).AddTicks(5640));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 20, 15, 30, 14, 837, DateTimeKind.Local).AddTicks(5640),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 20, 15, 32, 1, 302, DateTimeKind.Local).AddTicks(3104));

            migrationBuilder.AddColumn<int>(
                name: "PdfPageId",
                table: "Notes",
                type: "INTEGER",
                nullable: true,
                defaultValue: 0);
        }
    }
}
