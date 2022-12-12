using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    /// <inheritdoc />
    public partial class lastPreviewUpdate5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "resultStatus",
                table: "Results");

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_updateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 12, 2, 10, 39, 12, 533, DateTimeKind.Local).AddTicks(3399),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 12, 2, 10, 18, 28, 869, DateTimeKind.Local).AddTicks(2922));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_createDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 12, 2, 10, 39, 12, 533, DateTimeKind.Local).AddTicks(3237),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 12, 2, 10, 18, 28, 869, DateTimeKind.Local).AddTicks(2794));

            migrationBuilder.AlterColumn<DateTime>(
                name: "sendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 12, 2, 10, 39, 12, 533, DateTimeKind.Local).AddTicks(7662),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 12, 2, 10, 18, 28, 869, DateTimeKind.Local).AddTicks(7176));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "user_updateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 12, 2, 10, 18, 28, 869, DateTimeKind.Local).AddTicks(2922),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 12, 2, 10, 39, 12, 533, DateTimeKind.Local).AddTicks(3399));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_createDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 12, 2, 10, 18, 28, 869, DateTimeKind.Local).AddTicks(2794),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 12, 2, 10, 39, 12, 533, DateTimeKind.Local).AddTicks(3237));

            migrationBuilder.AddColumn<string>(
                name: "resultStatus",
                table: "Results",
                type: "TEXT",
                nullable: false,
                defaultValue: "#ADB5BD");

            migrationBuilder.AlterColumn<DateTime>(
                name: "sendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 12, 2, 10, 18, 28, 869, DateTimeKind.Local).AddTicks(7176),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 12, 2, 10, 39, 12, 533, DateTimeKind.Local).AddTicks(7662));
        }
    }
}
