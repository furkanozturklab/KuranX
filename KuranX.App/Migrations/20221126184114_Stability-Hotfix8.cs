using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    /// <inheritdoc />
    public partial class StabilityHotfix8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "user_location",
                table: "Users");

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_updateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 21, 41, 14, 53, DateTimeKind.Local).AddTicks(2487),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 26, 21, 37, 36, 724, DateTimeKind.Local).AddTicks(1856));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_createDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 21, 41, 14, 53, DateTimeKind.Local).AddTicks(2318),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 26, 21, 37, 36, 724, DateTimeKind.Local).AddTicks(1672));

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

            migrationBuilder.AlterColumn<DateTime>(
                name: "sendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 21, 41, 14, 53, DateTimeKind.Local).AddTicks(6840),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 26, 21, 37, 36, 724, DateTimeKind.Local).AddTicks(6120));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "user_city",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "user_country",
                table: "Users");

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_updateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 21, 37, 36, 724, DateTimeKind.Local).AddTicks(1856),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 26, 21, 41, 14, 53, DateTimeKind.Local).AddTicks(2487));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_createDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 21, 37, 36, 724, DateTimeKind.Local).AddTicks(1672),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 26, 21, 41, 14, 53, DateTimeKind.Local).AddTicks(2318));

            migrationBuilder.AddColumn<string>(
                name: "user_location",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "default");

            migrationBuilder.AlterColumn<DateTime>(
                name: "sendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 21, 37, 36, 724, DateTimeKind.Local).AddTicks(6120),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 26, 21, 41, 14, 53, DateTimeKind.Local).AddTicks(6840));
        }
    }
}
