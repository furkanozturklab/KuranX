using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    /// <inheritdoc />
    public partial class StabilityHotfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "user_updateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 24, 21, 23, 38, 463, DateTimeKind.Local).AddTicks(6156),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "user_screetQuestion",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "Screet Question",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: "Screet Question");

            migrationBuilder.AlterColumn<string>(
                name: "user_password",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "1230",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "user_lastName",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "Last Name",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: "Last Name");

            migrationBuilder.AlterColumn<string>(
                name: "user_fisrtName",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "First Name",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: "First Name");

            migrationBuilder.AlterColumn<string>(
                name: "user_email",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "none",
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_createDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 24, 21, 23, 38, 463, DateTimeKind.Local).AddTicks(6009),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "user_avatarUrl",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "default",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: "default");

            migrationBuilder.AlterColumn<bool>(
                name: "ResultSubject",
                table: "Results",
                type: "INTEGER",
                nullable: false,
                defaultValue: false,
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

            migrationBuilder.AlterColumn<bool>(
                name: "ResultNotes",
                table: "Results",
                type: "INTEGER",
                nullable: false,
                defaultValue: false,
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

            migrationBuilder.AlterColumn<bool>(
                name: "ResultLib",
                table: "Results",
                type: "INTEGER",
                nullable: false,
                defaultValue: false,
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

            migrationBuilder.AlterColumn<DateTime>(
                name: "SendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 24, 21, 23, 38, 464, DateTimeKind.Local).AddTicks(553),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 20, 19, 10, 36, 742, DateTimeKind.Local).AddTicks(3223));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "user_updateDate",
                table: "Users",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 24, 21, 23, 38, 463, DateTimeKind.Local).AddTicks(6156));

            migrationBuilder.AlterColumn<string>(
                name: "user_screetQuestion",
                table: "Users",
                type: "TEXT",
                nullable: true,
                defaultValue: "Screet Question",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "Screet Question");

            migrationBuilder.AlterColumn<string>(
                name: "user_password",
                table: "Users",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "1230");

            migrationBuilder.AlterColumn<string>(
                name: "user_lastName",
                table: "Users",
                type: "TEXT",
                nullable: true,
                defaultValue: "Last Name",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "Last Name");

            migrationBuilder.AlterColumn<string>(
                name: "user_fisrtName",
                table: "Users",
                type: "TEXT",
                nullable: true,
                defaultValue: "First Name",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "First Name");

            migrationBuilder.AlterColumn<string>(
                name: "user_email",
                table: "Users",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "none");

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_createDate",
                table: "Users",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 24, 21, 23, 38, 463, DateTimeKind.Local).AddTicks(6009));

            migrationBuilder.AlterColumn<string>(
                name: "user_avatarUrl",
                table: "Users",
                type: "TEXT",
                nullable: true,
                defaultValue: "default",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "default");

            migrationBuilder.AlterColumn<bool>(
                name: "ResultSubject",
                table: "Results",
                type: "INTEGER",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldDefaultValue: false);

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
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldDefaultValue: false);

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
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldDefaultValue: false);

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
                oldDefaultValue: new DateTime(2022, 11, 24, 21, 23, 38, 464, DateTimeKind.Local).AddTicks(553));
        }
    }
}
