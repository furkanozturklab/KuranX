using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    /// <inheritdoc />
    public partial class MainBuildHotfix5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResultVerse",
                table: "Results");

            migrationBuilder.AlterColumn<bool>(
                name: "ResultSubject",
                table: "Results",
                type: "INTEGER",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "false");

            migrationBuilder.AlterColumn<string>(
                name: "ResultStatus",
                table: "Results",
                type: "TEXT",
                nullable: true,
                defaultValue: "#ADB5BD",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "#ADB5BD");

            migrationBuilder.AlterColumn<bool>(
                name: "ResultNotes",
                table: "Results",
                type: "INTEGER",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "false");

            migrationBuilder.AlterColumn<string>(
                name: "ResultName",
                table: "Results",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<bool>(
                name: "ResultLib",
                table: "Results",
                type: "INTEGER",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "false");

            migrationBuilder.AlterColumn<string>(
                name: "ResultFinallyNote",
                table: "Results",
                type: "TEXT",
                nullable: true,
                defaultValue: "Sonuç Metninizi buraya yaza bilirsiniz.",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "Sonuç Metninizi buraya yaza bilirsiniz.");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 20, 19, 10, 36, 742, DateTimeKind.Local).AddTicks(3223),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 20, 15, 32, 1, 302, DateTimeKind.Local).AddTicks(3104));

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Remider",
                type: "TEXT",
                nullable: true,
                defaultValue: "Wait",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "Wait");

            migrationBuilder.AlterColumn<string>(
                name: "RemiderName",
                table: "Remider",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "RemiderDetail",
                table: "Remider",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "LoopType",
                table: "Remider",
                type: "TEXT",
                nullable: true,
                defaultValue: "False",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "False");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ResultSubject",
                table: "Results",
                type: "TEXT",
                nullable: false,
                defaultValue: "false",
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "ResultStatus",
                table: "Results",
                type: "TEXT",
                nullable: false,
                defaultValue: "#ADB5BD",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: "#ADB5BD");

            migrationBuilder.AlterColumn<string>(
                name: "ResultNotes",
                table: "Results",
                type: "TEXT",
                nullable: false,
                defaultValue: "false",
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "ResultName",
                table: "Results",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ResultLib",
                table: "Results",
                type: "TEXT",
                nullable: false,
                defaultValue: "false",
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "ResultFinallyNote",
                table: "Results",
                type: "TEXT",
                nullable: false,
                defaultValue: "Sonuç Metninizi buraya yaza bilirsiniz.",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: "Sonuç Metninizi buraya yaza bilirsiniz.");

            migrationBuilder.AddColumn<string>(
                name: "ResultVerse",
                table: "Results",
                type: "TEXT",
                nullable: false,
                defaultValue: "false");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 20, 15, 32, 1, 302, DateTimeKind.Local).AddTicks(3104),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 20, 19, 10, 36, 742, DateTimeKind.Local).AddTicks(3223));

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Remider",
                type: "TEXT",
                nullable: false,
                defaultValue: "Wait",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: "Wait");

            migrationBuilder.AlterColumn<string>(
                name: "RemiderName",
                table: "Remider",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RemiderDetail",
                table: "Remider",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LoopType",
                table: "Remider",
                type: "TEXT",
                nullable: false,
                defaultValue: "False",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: "False");
        }
    }
}
