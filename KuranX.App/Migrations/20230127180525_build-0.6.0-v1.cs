using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    /// <inheritdoc />
    public partial class build060v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "user_updateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 27, 21, 5, 25, 438, DateTimeKind.Local).AddTicks(8447),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 1, 26, 9, 52, 14, 254, DateTimeKind.Local).AddTicks(4554));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_createDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 27, 21, 5, 25, 438, DateTimeKind.Local).AddTicks(8310),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 1, 26, 9, 52, 14, 254, DateTimeKind.Local).AddTicks(4380));

            migrationBuilder.AddColumn<int>(
                name: "numberOfSection",
                table: "Sure",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "sendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 27, 21, 5, 25, 439, DateTimeKind.Local).AddTicks(1955),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 1, 26, 9, 52, 14, 254, DateTimeKind.Local).AddTicks(8399));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "numberOfSection",
                table: "Sure");

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_updateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 26, 9, 52, 14, 254, DateTimeKind.Local).AddTicks(4554),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 1, 27, 21, 5, 25, 438, DateTimeKind.Local).AddTicks(8447));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_createDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 26, 9, 52, 14, 254, DateTimeKind.Local).AddTicks(4380),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 1, 27, 21, 5, 25, 438, DateTimeKind.Local).AddTicks(8310));

            migrationBuilder.AlterColumn<DateTime>(
                name: "sendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 26, 9, 52, 14, 254, DateTimeKind.Local).AddTicks(8399),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 1, 27, 21, 5, 25, 439, DateTimeKind.Local).AddTicks(1955));
        }
    }
}
