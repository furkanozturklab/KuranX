using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    /// <inheritdoc />
    public partial class firstMainBuildHofix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "user_city",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "user_country",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "user_password",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "user_phone",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "user_screetQuestion",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "screetQuestionAnswer",
                table: "Users",
                newName: "pin");

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_updateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 29, 19, 8, 39, 454, DateTimeKind.Local).AddTicks(4756),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 26, 21, 41, 14, 53, DateTimeKind.Local).AddTicks(2487));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_createDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 29, 19, 8, 39, 454, DateTimeKind.Local).AddTicks(4609),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 26, 21, 41, 14, 53, DateTimeKind.Local).AddTicks(2318));

            migrationBuilder.AlterColumn<DateTime>(
                name: "sendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 29, 19, 8, 39, 454, DateTimeKind.Local).AddTicks(9149),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 26, 21, 41, 14, 53, DateTimeKind.Local).AddTicks(6840));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "pin",
                table: "Users",
                newName: "screetQuestionAnswer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_updateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 21, 41, 14, 53, DateTimeKind.Local).AddTicks(2487),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 29, 19, 8, 39, 454, DateTimeKind.Local).AddTicks(4756));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_createDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 21, 41, 14, 53, DateTimeKind.Local).AddTicks(2318),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 29, 19, 8, 39, 454, DateTimeKind.Local).AddTicks(4609));

            migrationBuilder.AddColumn<string>(
                name: "user_city",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "user_country",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "user_password",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "1230");

            migrationBuilder.AddColumn<string>(
                name: "user_phone",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "user_screetQuestion",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "Screet Question");

            migrationBuilder.AlterColumn<DateTime>(
                name: "sendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 21, 41, 14, 53, DateTimeKind.Local).AddTicks(6840),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 29, 19, 8, 39, 454, DateTimeKind.Local).AddTicks(9149));
        }
    }
}
