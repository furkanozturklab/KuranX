using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    /// <inheritdoc />
    public partial class lastPreviewUpdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "screetQuestionAnw",
                table: "Users",
                newName: "user_screetAnw");

            migrationBuilder.RenameColumn(
                name: "screetQuestion",
                table: "Users",
                newName: "user_screetQuestion");

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_updateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 30, 21, 11, 25, 40, DateTimeKind.Local).AddTicks(9132),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 30, 21, 10, 1, 994, DateTimeKind.Local).AddTicks(1149));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_createDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 30, 21, 11, 25, 40, DateTimeKind.Local).AddTicks(8850),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 30, 21, 10, 1, 994, DateTimeKind.Local).AddTicks(1008));

            migrationBuilder.AlterColumn<string>(
                name: "user_screetAnw",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "Yeni Değeri Girin",
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "user_screetQuestion",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "Değiştiriniz",
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "sendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 30, 21, 11, 25, 41, DateTimeKind.Local).AddTicks(4354),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 30, 21, 10, 1, 994, DateTimeKind.Local).AddTicks(5515));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "user_screetQuestion",
                table: "Users",
                newName: "screetQuestion");

            migrationBuilder.RenameColumn(
                name: "user_screetAnw",
                table: "Users",
                newName: "screetQuestionAnw");

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_updateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 30, 21, 10, 1, 994, DateTimeKind.Local).AddTicks(1149),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 30, 21, 11, 25, 40, DateTimeKind.Local).AddTicks(9132));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_createDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 30, 21, 10, 1, 994, DateTimeKind.Local).AddTicks(1008),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 30, 21, 11, 25, 40, DateTimeKind.Local).AddTicks(8850));

            migrationBuilder.AlterColumn<string>(
                name: "screetQuestion",
                table: "Users",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "Değiştiriniz");

            migrationBuilder.AlterColumn<string>(
                name: "screetQuestionAnw",
                table: "Users",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "Yeni Değeri Girin");

            migrationBuilder.AlterColumn<DateTime>(
                name: "sendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 30, 21, 10, 1, 994, DateTimeKind.Local).AddTicks(5515),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 30, 21, 11, 25, 41, DateTimeKind.Local).AddTicks(4354));
        }
    }
}
