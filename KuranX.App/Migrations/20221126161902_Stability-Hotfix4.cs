using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    /// <inheritdoc />
    public partial class StabilityHotfix4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "commentary",
                table: "Verse",
                type: "TEXT",
                nullable: false,
                defaultValue: "Wait");

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_updateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 19, 19, 1, 907, DateTimeKind.Local).AddTicks(8235),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 26, 16, 50, 46, 390, DateTimeKind.Local).AddTicks(8396));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_createDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 19, 19, 1, 907, DateTimeKind.Local).AddTicks(8078),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 26, 16, 50, 46, 390, DateTimeKind.Local).AddTicks(8272));

            migrationBuilder.AlterColumn<DateTime>(
                name: "sendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 19, 19, 1, 908, DateTimeKind.Local).AddTicks(2028),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 26, 16, 50, 46, 391, DateTimeKind.Local).AddTicks(2058));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "commentary",
                table: "Verse");

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_updateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 16, 50, 46, 390, DateTimeKind.Local).AddTicks(8396),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 26, 19, 19, 1, 907, DateTimeKind.Local).AddTicks(8235));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_createDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 16, 50, 46, 390, DateTimeKind.Local).AddTicks(8272),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 26, 19, 19, 1, 907, DateTimeKind.Local).AddTicks(8078));

            migrationBuilder.AlterColumn<DateTime>(
                name: "sendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 16, 50, 46, 391, DateTimeKind.Local).AddTicks(2058),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 26, 19, 19, 1, 908, DateTimeKind.Local).AddTicks(2028));
        }
    }
}
