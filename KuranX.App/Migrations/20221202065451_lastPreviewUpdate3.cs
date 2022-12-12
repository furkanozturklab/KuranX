using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    /// <inheritdoc />
    public partial class lastPreviewUpdate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "user_updateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 12, 2, 9, 54, 51, 371, DateTimeKind.Local).AddTicks(2415),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 30, 21, 11, 25, 40, DateTimeKind.Local).AddTicks(9132));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_createDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 12, 2, 9, 54, 51, 371, DateTimeKind.Local).AddTicks(2294),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 30, 21, 11, 25, 40, DateTimeKind.Local).AddTicks(8850));

            migrationBuilder.AlterColumn<DateTime>(
                name: "sendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 12, 2, 9, 54, 51, 371, DateTimeKind.Local).AddTicks(6683),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 30, 21, 11, 25, 41, DateTimeKind.Local).AddTicks(4354));

            migrationBuilder.AddColumn<bool>(
                name: "integrityProtected",
                table: "Integrity",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "integrityProtected",
                table: "Integrity");

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_updateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 30, 21, 11, 25, 40, DateTimeKind.Local).AddTicks(9132),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 12, 2, 9, 54, 51, 371, DateTimeKind.Local).AddTicks(2415));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_createDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 30, 21, 11, 25, 40, DateTimeKind.Local).AddTicks(8850),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 12, 2, 9, 54, 51, 371, DateTimeKind.Local).AddTicks(2294));

            migrationBuilder.AlterColumn<DateTime>(
                name: "sendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 30, 21, 11, 25, 41, DateTimeKind.Local).AddTicks(4354),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 12, 2, 9, 54, 51, 371, DateTimeKind.Local).AddTicks(6683));
        }
    }
}
