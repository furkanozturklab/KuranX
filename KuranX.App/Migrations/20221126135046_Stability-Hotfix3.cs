using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    /// <inheritdoc />
    public partial class StabilityHotfix3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "wordText",
                table: "Words",
                newName: "word_meal");

            migrationBuilder.RenameColumn(
                name: "wordRe",
                table: "Words",
                newName: "tr_read");

            migrationBuilder.AddColumn<string>(
                name: "arp_read",
                table: "Words",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "root",
                table: "Words",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_updateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 16, 50, 46, 390, DateTimeKind.Local).AddTicks(8396),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 24, 22, 33, 27, 513, DateTimeKind.Local).AddTicks(6191));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_createDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 16, 50, 46, 390, DateTimeKind.Local).AddTicks(8272),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 24, 22, 33, 27, 513, DateTimeKind.Local).AddTicks(6052));

            migrationBuilder.AlterColumn<int>(
                name: "verseId",
                table: "SubjectItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "sureId",
                table: "SubjectItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "subjectNotesId",
                table: "SubjectItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "subjectName",
                table: "SubjectItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "subjectId",
                table: "SubjectItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "subjectName",
                table: "Subject",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "subjectColor",
                table: "Subject",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "sendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 26, 16, 50, 46, 391, DateTimeKind.Local).AddTicks(2058),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 24, 22, 33, 27, 514, DateTimeKind.Local).AddTicks(470));

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "Remider",
                type: "TEXT",
                nullable: false,
                defaultValue: "Wait",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: "Wait");

            migrationBuilder.AlterColumn<string>(
                name: "remiderName",
                table: "Remider",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "remiderDetail",
                table: "Remider",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "loopType",
                table: "Remider",
                type: "TEXT",
                nullable: false,
                defaultValue: "False",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: "False");

            migrationBuilder.AlterColumn<int>(
                name: "verseId",
                table: "Notes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true,
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "sureId",
                table: "Notes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true,
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "subjectId",
                table: "Notes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true,
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "pdfFileId",
                table: "Notes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true,
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "noteLocation",
                table: "Notes",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "noteHeader",
                table: "Notes",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "noteDetail",
                table: "Notes",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "libraryId",
                table: "Notes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true,
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "libraryName",
                table: "Librarys",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "libraryColor",
                table: "Librarys",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "verseId",
                table: "Interpreter",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "sureId",
                table: "Interpreter",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "interpreterWriter",
                table: "Interpreter",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "interpreterDetail",
                table: "Interpreter",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "integrityNote",
                table: "Integrity",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "integrityName",
                table: "Integrity",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "connectedVerseId",
                table: "Integrity",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "connectedSureId",
                table: "Integrity",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "connectVerseId",
                table: "Integrity",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "connectSureId",
                table: "Integrity",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "arp_read",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "root",
                table: "Words");

            migrationBuilder.RenameColumn(
                name: "word_meal",
                table: "Words",
                newName: "wordText");

            migrationBuilder.RenameColumn(
                name: "tr_read",
                table: "Words",
                newName: "wordRe");

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_updateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 24, 22, 33, 27, 513, DateTimeKind.Local).AddTicks(6191),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 26, 16, 50, 46, 390, DateTimeKind.Local).AddTicks(8396));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_createDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 24, 22, 33, 27, 513, DateTimeKind.Local).AddTicks(6052),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 26, 16, 50, 46, 390, DateTimeKind.Local).AddTicks(8272));

            migrationBuilder.AlterColumn<int>(
                name: "verseId",
                table: "SubjectItems",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "sureId",
                table: "SubjectItems",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "subjectNotesId",
                table: "SubjectItems",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "subjectName",
                table: "SubjectItems",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "subjectId",
                table: "SubjectItems",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "subjectName",
                table: "Subject",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "subjectColor",
                table: "Subject",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "sendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 24, 22, 33, 27, 514, DateTimeKind.Local).AddTicks(470),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 26, 16, 50, 46, 391, DateTimeKind.Local).AddTicks(2058));

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "Remider",
                type: "TEXT",
                nullable: true,
                defaultValue: "Wait",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "Wait");

            migrationBuilder.AlterColumn<string>(
                name: "remiderName",
                table: "Remider",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "remiderDetail",
                table: "Remider",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "loopType",
                table: "Remider",
                type: "TEXT",
                nullable: true,
                defaultValue: "False",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "False");

            migrationBuilder.AlterColumn<int>(
                name: "verseId",
                table: "Notes",
                type: "INTEGER",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "sureId",
                table: "Notes",
                type: "INTEGER",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "subjectId",
                table: "Notes",
                type: "INTEGER",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "pdfFileId",
                table: "Notes",
                type: "INTEGER",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "noteLocation",
                table: "Notes",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "noteHeader",
                table: "Notes",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "noteDetail",
                table: "Notes",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "libraryId",
                table: "Notes",
                type: "INTEGER",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "libraryName",
                table: "Librarys",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "libraryColor",
                table: "Librarys",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "verseId",
                table: "Interpreter",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "sureId",
                table: "Interpreter",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "interpreterWriter",
                table: "Interpreter",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "interpreterDetail",
                table: "Interpreter",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "integrityNote",
                table: "Integrity",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "integrityName",
                table: "Integrity",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "connectedVerseId",
                table: "Integrity",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "connectedSureId",
                table: "Integrity",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "connectVerseId",
                table: "Integrity",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "connectSureId",
                table: "Integrity",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
