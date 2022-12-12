using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    /// <inheritdoc />
    public partial class lastPreviewUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "user_updateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 30, 21, 10, 1, 994, DateTimeKind.Local).AddTicks(1149),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 29, 19, 8, 39, 454, DateTimeKind.Local).AddTicks(4756));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_createDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 30, 21, 10, 1, 994, DateTimeKind.Local).AddTicks(1008),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 29, 19, 8, 39, 454, DateTimeKind.Local).AddTicks(4609));

            migrationBuilder.AddColumn<string>(
                name: "screetQuestion",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "screetQuestionAnw",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "sendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 30, 21, 10, 1, 994, DateTimeKind.Local).AddTicks(5515),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 29, 19, 8, 39, 454, DateTimeKind.Local).AddTicks(9149));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "screetQuestion",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "screetQuestionAnw",
                table: "Users");

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_updateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 29, 19, 8, 39, 454, DateTimeKind.Local).AddTicks(4756),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 30, 21, 10, 1, 994, DateTimeKind.Local).AddTicks(1149));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_createDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 29, 19, 8, 39, 454, DateTimeKind.Local).AddTicks(4609),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 30, 21, 10, 1, 994, DateTimeKind.Local).AddTicks(1008));

            migrationBuilder.AlterColumn<DateTime>(
                name: "sendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 29, 19, 8, 39, 454, DateTimeKind.Local).AddTicks(9149),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 30, 21, 10, 1, 994, DateTimeKind.Local).AddTicks(5515));
        }
    }
}
