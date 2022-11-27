using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    /// <inheritdoc />
    public partial class StabilityHotfix7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "user_updateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 21, 37, 36, 724, DateTimeKind.Local).AddTicks(1856),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 26, 21, 13, 1, 195, DateTimeKind.Local).AddTicks(2773));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_createDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 21, 37, 36, 724, DateTimeKind.Local).AddTicks(1672),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 26, 21, 13, 1, 195, DateTimeKind.Local).AddTicks(2594));

            migrationBuilder.AddColumn<string>(
                name: "user_phone",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "sendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 21, 37, 36, 724, DateTimeKind.Local).AddTicks(6120),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 26, 21, 13, 1, 195, DateTimeKind.Local).AddTicks(6646));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "user_phone",
                table: "Users");

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_updateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 21, 13, 1, 195, DateTimeKind.Local).AddTicks(2773),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 26, 21, 37, 36, 724, DateTimeKind.Local).AddTicks(1856));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_createDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 21, 13, 1, 195, DateTimeKind.Local).AddTicks(2594),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 26, 21, 37, 36, 724, DateTimeKind.Local).AddTicks(1672));

            migrationBuilder.AlterColumn<DateTime>(
                name: "sendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 21, 13, 1, 195, DateTimeKind.Local).AddTicks(6646),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 26, 21, 37, 36, 724, DateTimeKind.Local).AddTicks(6120));
        }
    }
}
