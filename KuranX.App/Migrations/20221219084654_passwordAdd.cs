using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    /// <inheritdoc />
    public partial class passwordAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "user_updateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 12, 19, 11, 46, 54, 248, DateTimeKind.Local).AddTicks(2494),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 12, 12, 20, 18, 3, 626, DateTimeKind.Local).AddTicks(6299));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_createDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 12, 19, 11, 46, 54, 248, DateTimeKind.Local).AddTicks(2356),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 12, 12, 20, 18, 3, 626, DateTimeKind.Local).AddTicks(6146));

            migrationBuilder.AlterColumn<DateTime>(
                name: "sendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 12, 19, 11, 46, 54, 248, DateTimeKind.Local).AddTicks(7174),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 12, 12, 20, 18, 3, 627, DateTimeKind.Local).AddTicks(1122));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "user_updateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 12, 12, 20, 18, 3, 626, DateTimeKind.Local).AddTicks(6299),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 12, 19, 11, 46, 54, 248, DateTimeKind.Local).AddTicks(2494));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_createDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 12, 12, 20, 18, 3, 626, DateTimeKind.Local).AddTicks(6146),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 12, 19, 11, 46, 54, 248, DateTimeKind.Local).AddTicks(2356));

            migrationBuilder.AlterColumn<DateTime>(
                name: "sendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 12, 12, 20, 18, 3, 627, DateTimeKind.Local).AddTicks(1122),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 12, 19, 11, 46, 54, 248, DateTimeKind.Local).AddTicks(7174));
        }
    }
}
