using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    /// <inheritdoc />
    public partial class StabilityHotfix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WordText",
                table: "Words",
                newName: "wordText");

            migrationBuilder.RenameColumn(
                name: "WordRe",
                table: "Words",
                newName: "wordRe");

            migrationBuilder.RenameColumn(
                name: "VerseId",
                table: "Words",
                newName: "verseId");

            migrationBuilder.RenameColumn(
                name: "SureId",
                table: "Words",
                newName: "sureId");

            migrationBuilder.RenameColumn(
                name: "WordsId",
                table: "Words",
                newName: "wordsId");

            migrationBuilder.RenameColumn(
                name: "VerseTr",
                table: "Verse",
                newName: "verseTr");

            migrationBuilder.RenameColumn(
                name: "VerseDesc",
                table: "Verse",
                newName: "verseDesc");

            migrationBuilder.RenameColumn(
                name: "VerseCheck",
                table: "Verse",
                newName: "verseCheck");

            migrationBuilder.RenameColumn(
                name: "VerseArabic",
                table: "Verse",
                newName: "verseArabic");

            migrationBuilder.RenameColumn(
                name: "SureId",
                table: "Verse",
                newName: "sureId");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Verse",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "RemiderCheck",
                table: "Verse",
                newName: "remiderCheck");

            migrationBuilder.RenameColumn(
                name: "RelativeDesk",
                table: "Verse",
                newName: "relativeDesk");

            migrationBuilder.RenameColumn(
                name: "MarkCheck",
                table: "Verse",
                newName: "markCheck");

            migrationBuilder.RenameColumn(
                name: "VerseId",
                table: "Verse",
                newName: "verseId");

            migrationBuilder.RenameColumn(
                name: "MissonsType",
                table: "Tasks",
                newName: "missonsType");

            migrationBuilder.RenameColumn(
                name: "MissonsTime",
                table: "Tasks",
                newName: "missonsTime");

            migrationBuilder.RenameColumn(
                name: "MissonsRepeart",
                table: "Tasks",
                newName: "missonsRepeart");

            migrationBuilder.RenameColumn(
                name: "MissonsId",
                table: "Tasks",
                newName: "missonsId");

            migrationBuilder.RenameColumn(
                name: "MissonsColor",
                table: "Tasks",
                newName: "missonsColor");

            migrationBuilder.RenameColumn(
                name: "TasksId",
                table: "Tasks",
                newName: "tasksId");

            migrationBuilder.RenameColumn(
                name: "UserLastRelativeVerse",
                table: "Sure",
                newName: "userLastRelativeVerse");

            migrationBuilder.RenameColumn(
                name: "UserCheckCount",
                table: "Sure",
                newName: "userCheckCount");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Sure",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "NumberOfVerses",
                table: "Sure",
                newName: "numberOfVerses");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Sure",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "LandingLocation",
                table: "Sure",
                newName: "landingLocation");

            migrationBuilder.RenameColumn(
                name: "DeskMushaf",
                table: "Sure",
                newName: "deskMushaf");

            migrationBuilder.RenameColumn(
                name: "DeskList",
                table: "Sure",
                newName: "deskList");

            migrationBuilder.RenameColumn(
                name: "DeskLanding",
                table: "Sure",
                newName: "deskLanding");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Sure",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Complated",
                table: "Sure",
                newName: "complated");

            migrationBuilder.RenameColumn(
                name: "VerseId",
                table: "SubjectItems",
                newName: "verseId");

            migrationBuilder.RenameColumn(
                name: "SureId",
                table: "SubjectItems",
                newName: "sureId");

            migrationBuilder.RenameColumn(
                name: "SubjectNotesId",
                table: "SubjectItems",
                newName: "subjectNotesId");

            migrationBuilder.RenameColumn(
                name: "SubjectName",
                table: "SubjectItems",
                newName: "subjectName");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "SubjectItems",
                newName: "subjectId");

            migrationBuilder.RenameColumn(
                name: "Modify",
                table: "SubjectItems",
                newName: "modify");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "SubjectItems",
                newName: "created");

            migrationBuilder.RenameColumn(
                name: "SubjectItemsId",
                table: "SubjectItems",
                newName: "subjectItemsId");

            migrationBuilder.RenameColumn(
                name: "SubjectName",
                table: "Subject",
                newName: "subjectName");

            migrationBuilder.RenameColumn(
                name: "SubjectColor",
                table: "Subject",
                newName: "subjectColor");

            migrationBuilder.RenameColumn(
                name: "Modify",
                table: "Subject",
                newName: "modify");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Subject",
                newName: "created");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "Subject",
                newName: "subjectId");

            migrationBuilder.RenameColumn(
                name: "ResultSubject",
                table: "Results",
                newName: "resultSubject");

            migrationBuilder.RenameColumn(
                name: "ResultStatus",
                table: "Results",
                newName: "resultStatus");

            migrationBuilder.RenameColumn(
                name: "ResultNotes",
                table: "Results",
                newName: "resultNotes");

            migrationBuilder.RenameColumn(
                name: "ResultName",
                table: "Results",
                newName: "resultName");

            migrationBuilder.RenameColumn(
                name: "ResultLib",
                table: "Results",
                newName: "resultLib");

            migrationBuilder.RenameColumn(
                name: "ResultFinallyNote",
                table: "Results",
                newName: "resultFinallyNote");

            migrationBuilder.RenameColumn(
                name: "ResultId",
                table: "Results",
                newName: "resultId");

            migrationBuilder.RenameColumn(
                name: "SendTime",
                table: "ResultItems",
                newName: "sendTime");

            migrationBuilder.RenameColumn(
                name: "ResultSubjectId",
                table: "ResultItems",
                newName: "resultSubjectId");

            migrationBuilder.RenameColumn(
                name: "ResultNoteId",
                table: "ResultItems",
                newName: "resultNoteId");

            migrationBuilder.RenameColumn(
                name: "ResultLibId",
                table: "ResultItems",
                newName: "resultLibId");

            migrationBuilder.RenameColumn(
                name: "ResultId",
                table: "ResultItems",
                newName: "resultId");

            migrationBuilder.RenameColumn(
                name: "ResultItemId",
                table: "ResultItems",
                newName: "resultItemId");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Remider",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "RemiderName",
                table: "Remider",
                newName: "remiderName");

            migrationBuilder.RenameColumn(
                name: "RemiderDetail",
                table: "Remider",
                newName: "remiderDetail");

            migrationBuilder.RenameColumn(
                name: "RemiderDate",
                table: "Remider",
                newName: "remiderDate");

            migrationBuilder.RenameColumn(
                name: "Priority",
                table: "Remider",
                newName: "priority");

            migrationBuilder.RenameColumn(
                name: "LoopType",
                table: "Remider",
                newName: "loopType");

            migrationBuilder.RenameColumn(
                name: "LastAction",
                table: "Remider",
                newName: "lastAction");

            migrationBuilder.RenameColumn(
                name: "Create",
                table: "Remider",
                newName: "create");

            migrationBuilder.RenameColumn(
                name: "ConnectVerseId",
                table: "Remider",
                newName: "connectVerseId");

            migrationBuilder.RenameColumn(
                name: "ConnectSureId",
                table: "Remider",
                newName: "connectSureId");

            migrationBuilder.RenameColumn(
                name: "RemiderId",
                table: "Remider",
                newName: "remiderId");

            migrationBuilder.RenameColumn(
                name: "Modify",
                table: "PdfFile",
                newName: "modify");

            migrationBuilder.RenameColumn(
                name: "FileUrl",
                table: "PdfFile",
                newName: "fileUrl");

            migrationBuilder.RenameColumn(
                name: "FileType",
                table: "PdfFile",
                newName: "fileType");

            migrationBuilder.RenameColumn(
                name: "FileSize",
                table: "PdfFile",
                newName: "fileSize");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "PdfFile",
                newName: "fileName");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "PdfFile",
                newName: "created");

            migrationBuilder.RenameColumn(
                name: "PdfFileId",
                table: "PdfFile",
                newName: "pdfFileId");

            migrationBuilder.RenameColumn(
                name: "VerseId",
                table: "Notes",
                newName: "verseId");

            migrationBuilder.RenameColumn(
                name: "SureId",
                table: "Notes",
                newName: "sureId");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "Notes",
                newName: "subjectId");

            migrationBuilder.RenameColumn(
                name: "PdfFileId",
                table: "Notes",
                newName: "pdfFileId");

            migrationBuilder.RenameColumn(
                name: "NoteLocation",
                table: "Notes",
                newName: "noteLocation");

            migrationBuilder.RenameColumn(
                name: "NoteHeader",
                table: "Notes",
                newName: "noteHeader");

            migrationBuilder.RenameColumn(
                name: "NoteDetail",
                table: "Notes",
                newName: "noteDetail");

            migrationBuilder.RenameColumn(
                name: "Modify",
                table: "Notes",
                newName: "modify");

            migrationBuilder.RenameColumn(
                name: "LibraryId",
                table: "Notes",
                newName: "libraryId");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Notes",
                newName: "created");

            migrationBuilder.RenameColumn(
                name: "NotesId",
                table: "Notes",
                newName: "notesId");

            migrationBuilder.RenameColumn(
                name: "Modify",
                table: "Librarys",
                newName: "modify");

            migrationBuilder.RenameColumn(
                name: "LibraryName",
                table: "Librarys",
                newName: "libraryName");

            migrationBuilder.RenameColumn(
                name: "LibraryColor",
                table: "Librarys",
                newName: "libraryColor");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Librarys",
                newName: "created");

            migrationBuilder.RenameColumn(
                name: "LibraryId",
                table: "Librarys",
                newName: "libraryId");

            migrationBuilder.RenameColumn(
                name: "Modify",
                table: "Integrity",
                newName: "modify");

            migrationBuilder.RenameColumn(
                name: "IntegrityNote",
                table: "Integrity",
                newName: "integrityNote");

            migrationBuilder.RenameColumn(
                name: "IntegrityName",
                table: "Integrity",
                newName: "integrityName");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Integrity",
                newName: "created");

            migrationBuilder.RenameColumn(
                name: "IntegrityId",
                table: "Integrity",
                newName: "integrityId");

            migrationBuilder.AlterColumn<string>(
                name: "verseTr",
                table: "Verse",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "verseDesc",
                table: "Verse",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "verseCheck",
                table: "Verse",
                type: "INTEGER",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "verseArabic",
                table: "Verse",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "sureId",
                table: "Verse",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "Verse",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "remiderCheck",
                table: "Verse",
                type: "INTEGER",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "relativeDesk",
                table: "Verse",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "markCheck",
                table: "Verse",
                type: "INTEGER",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_updateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 24, 22, 33, 27, 513, DateTimeKind.Local).AddTicks(6191),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 24, 21, 23, 38, 463, DateTimeKind.Local).AddTicks(6156));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_createDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 24, 22, 33, 27, 513, DateTimeKind.Local).AddTicks(6052),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 24, 21, 23, 38, 463, DateTimeKind.Local).AddTicks(6009));

            migrationBuilder.AlterColumn<int>(
                name: "userLastRelativeVerse",
                table: "Sure",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true,
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "userCheckCount",
                table: "Sure",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true,
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "Sure",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "numberOfVerses",
                table: "Sure",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Sure",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "landingLocation",
                table: "Sure",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "deskMushaf",
                table: "Sure",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "deskList",
                table: "Sure",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true,
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "deskLanding",
                table: "Sure",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "Sure",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "complated",
                table: "Sure",
                type: "INTEGER",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "sendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 24, 22, 33, 27, 514, DateTimeKind.Local).AddTicks(470),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 24, 21, 23, 38, 464, DateTimeKind.Local).AddTicks(553));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "wordText",
                table: "Words",
                newName: "WordText");

            migrationBuilder.RenameColumn(
                name: "wordRe",
                table: "Words",
                newName: "WordRe");

            migrationBuilder.RenameColumn(
                name: "verseId",
                table: "Words",
                newName: "VerseId");

            migrationBuilder.RenameColumn(
                name: "sureId",
                table: "Words",
                newName: "SureId");

            migrationBuilder.RenameColumn(
                name: "wordsId",
                table: "Words",
                newName: "WordsId");

            migrationBuilder.RenameColumn(
                name: "verseTr",
                table: "Verse",
                newName: "VerseTr");

            migrationBuilder.RenameColumn(
                name: "verseDesc",
                table: "Verse",
                newName: "VerseDesc");

            migrationBuilder.RenameColumn(
                name: "verseCheck",
                table: "Verse",
                newName: "VerseCheck");

            migrationBuilder.RenameColumn(
                name: "verseArabic",
                table: "Verse",
                newName: "VerseArabic");

            migrationBuilder.RenameColumn(
                name: "sureId",
                table: "Verse",
                newName: "SureId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Verse",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "remiderCheck",
                table: "Verse",
                newName: "RemiderCheck");

            migrationBuilder.RenameColumn(
                name: "relativeDesk",
                table: "Verse",
                newName: "RelativeDesk");

            migrationBuilder.RenameColumn(
                name: "markCheck",
                table: "Verse",
                newName: "MarkCheck");

            migrationBuilder.RenameColumn(
                name: "verseId",
                table: "Verse",
                newName: "VerseId");

            migrationBuilder.RenameColumn(
                name: "missonsType",
                table: "Tasks",
                newName: "MissonsType");

            migrationBuilder.RenameColumn(
                name: "missonsTime",
                table: "Tasks",
                newName: "MissonsTime");

            migrationBuilder.RenameColumn(
                name: "missonsRepeart",
                table: "Tasks",
                newName: "MissonsRepeart");

            migrationBuilder.RenameColumn(
                name: "missonsId",
                table: "Tasks",
                newName: "MissonsId");

            migrationBuilder.RenameColumn(
                name: "missonsColor",
                table: "Tasks",
                newName: "MissonsColor");

            migrationBuilder.RenameColumn(
                name: "tasksId",
                table: "Tasks",
                newName: "TasksId");

            migrationBuilder.RenameColumn(
                name: "userLastRelativeVerse",
                table: "Sure",
                newName: "UserLastRelativeVerse");

            migrationBuilder.RenameColumn(
                name: "userCheckCount",
                table: "Sure",
                newName: "UserCheckCount");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Sure",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "numberOfVerses",
                table: "Sure",
                newName: "NumberOfVerses");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Sure",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "landingLocation",
                table: "Sure",
                newName: "LandingLocation");

            migrationBuilder.RenameColumn(
                name: "deskMushaf",
                table: "Sure",
                newName: "DeskMushaf");

            migrationBuilder.RenameColumn(
                name: "deskList",
                table: "Sure",
                newName: "DeskList");

            migrationBuilder.RenameColumn(
                name: "deskLanding",
                table: "Sure",
                newName: "DeskLanding");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Sure",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "complated",
                table: "Sure",
                newName: "Complated");

            migrationBuilder.RenameColumn(
                name: "verseId",
                table: "SubjectItems",
                newName: "VerseId");

            migrationBuilder.RenameColumn(
                name: "sureId",
                table: "SubjectItems",
                newName: "SureId");

            migrationBuilder.RenameColumn(
                name: "subjectNotesId",
                table: "SubjectItems",
                newName: "SubjectNotesId");

            migrationBuilder.RenameColumn(
                name: "subjectName",
                table: "SubjectItems",
                newName: "SubjectName");

            migrationBuilder.RenameColumn(
                name: "subjectId",
                table: "SubjectItems",
                newName: "SubjectId");

            migrationBuilder.RenameColumn(
                name: "modify",
                table: "SubjectItems",
                newName: "Modify");

            migrationBuilder.RenameColumn(
                name: "created",
                table: "SubjectItems",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "subjectItemsId",
                table: "SubjectItems",
                newName: "SubjectItemsId");

            migrationBuilder.RenameColumn(
                name: "subjectName",
                table: "Subject",
                newName: "SubjectName");

            migrationBuilder.RenameColumn(
                name: "subjectColor",
                table: "Subject",
                newName: "SubjectColor");

            migrationBuilder.RenameColumn(
                name: "modify",
                table: "Subject",
                newName: "Modify");

            migrationBuilder.RenameColumn(
                name: "created",
                table: "Subject",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "subjectId",
                table: "Subject",
                newName: "SubjectId");

            migrationBuilder.RenameColumn(
                name: "resultSubject",
                table: "Results",
                newName: "ResultSubject");

            migrationBuilder.RenameColumn(
                name: "resultStatus",
                table: "Results",
                newName: "ResultStatus");

            migrationBuilder.RenameColumn(
                name: "resultNotes",
                table: "Results",
                newName: "ResultNotes");

            migrationBuilder.RenameColumn(
                name: "resultName",
                table: "Results",
                newName: "ResultName");

            migrationBuilder.RenameColumn(
                name: "resultLib",
                table: "Results",
                newName: "ResultLib");

            migrationBuilder.RenameColumn(
                name: "resultFinallyNote",
                table: "Results",
                newName: "ResultFinallyNote");

            migrationBuilder.RenameColumn(
                name: "resultId",
                table: "Results",
                newName: "ResultId");

            migrationBuilder.RenameColumn(
                name: "sendTime",
                table: "ResultItems",
                newName: "SendTime");

            migrationBuilder.RenameColumn(
                name: "resultSubjectId",
                table: "ResultItems",
                newName: "ResultSubjectId");

            migrationBuilder.RenameColumn(
                name: "resultNoteId",
                table: "ResultItems",
                newName: "ResultNoteId");

            migrationBuilder.RenameColumn(
                name: "resultLibId",
                table: "ResultItems",
                newName: "ResultLibId");

            migrationBuilder.RenameColumn(
                name: "resultId",
                table: "ResultItems",
                newName: "ResultId");

            migrationBuilder.RenameColumn(
                name: "resultItemId",
                table: "ResultItems",
                newName: "ResultItemId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Remider",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "remiderName",
                table: "Remider",
                newName: "RemiderName");

            migrationBuilder.RenameColumn(
                name: "remiderDetail",
                table: "Remider",
                newName: "RemiderDetail");

            migrationBuilder.RenameColumn(
                name: "remiderDate",
                table: "Remider",
                newName: "RemiderDate");

            migrationBuilder.RenameColumn(
                name: "priority",
                table: "Remider",
                newName: "Priority");

            migrationBuilder.RenameColumn(
                name: "loopType",
                table: "Remider",
                newName: "LoopType");

            migrationBuilder.RenameColumn(
                name: "lastAction",
                table: "Remider",
                newName: "LastAction");

            migrationBuilder.RenameColumn(
                name: "create",
                table: "Remider",
                newName: "Create");

            migrationBuilder.RenameColumn(
                name: "connectVerseId",
                table: "Remider",
                newName: "ConnectVerseId");

            migrationBuilder.RenameColumn(
                name: "connectSureId",
                table: "Remider",
                newName: "ConnectSureId");

            migrationBuilder.RenameColumn(
                name: "remiderId",
                table: "Remider",
                newName: "RemiderId");

            migrationBuilder.RenameColumn(
                name: "modify",
                table: "PdfFile",
                newName: "Modify");

            migrationBuilder.RenameColumn(
                name: "fileUrl",
                table: "PdfFile",
                newName: "FileUrl");

            migrationBuilder.RenameColumn(
                name: "fileType",
                table: "PdfFile",
                newName: "FileType");

            migrationBuilder.RenameColumn(
                name: "fileSize",
                table: "PdfFile",
                newName: "FileSize");

            migrationBuilder.RenameColumn(
                name: "fileName",
                table: "PdfFile",
                newName: "FileName");

            migrationBuilder.RenameColumn(
                name: "created",
                table: "PdfFile",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "pdfFileId",
                table: "PdfFile",
                newName: "PdfFileId");

            migrationBuilder.RenameColumn(
                name: "verseId",
                table: "Notes",
                newName: "VerseId");

            migrationBuilder.RenameColumn(
                name: "sureId",
                table: "Notes",
                newName: "SureId");

            migrationBuilder.RenameColumn(
                name: "subjectId",
                table: "Notes",
                newName: "SubjectId");

            migrationBuilder.RenameColumn(
                name: "pdfFileId",
                table: "Notes",
                newName: "PdfFileId");

            migrationBuilder.RenameColumn(
                name: "noteLocation",
                table: "Notes",
                newName: "NoteLocation");

            migrationBuilder.RenameColumn(
                name: "noteHeader",
                table: "Notes",
                newName: "NoteHeader");

            migrationBuilder.RenameColumn(
                name: "noteDetail",
                table: "Notes",
                newName: "NoteDetail");

            migrationBuilder.RenameColumn(
                name: "modify",
                table: "Notes",
                newName: "Modify");

            migrationBuilder.RenameColumn(
                name: "libraryId",
                table: "Notes",
                newName: "LibraryId");

            migrationBuilder.RenameColumn(
                name: "created",
                table: "Notes",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "notesId",
                table: "Notes",
                newName: "NotesId");

            migrationBuilder.RenameColumn(
                name: "modify",
                table: "Librarys",
                newName: "Modify");

            migrationBuilder.RenameColumn(
                name: "libraryName",
                table: "Librarys",
                newName: "LibraryName");

            migrationBuilder.RenameColumn(
                name: "libraryColor",
                table: "Librarys",
                newName: "LibraryColor");

            migrationBuilder.RenameColumn(
                name: "created",
                table: "Librarys",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "libraryId",
                table: "Librarys",
                newName: "LibraryId");

            migrationBuilder.RenameColumn(
                name: "modify",
                table: "Integrity",
                newName: "Modify");

            migrationBuilder.RenameColumn(
                name: "integrityNote",
                table: "Integrity",
                newName: "IntegrityNote");

            migrationBuilder.RenameColumn(
                name: "integrityName",
                table: "Integrity",
                newName: "IntegrityName");

            migrationBuilder.RenameColumn(
                name: "created",
                table: "Integrity",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "integrityId",
                table: "Integrity",
                newName: "IntegrityId");

            migrationBuilder.AlterColumn<string>(
                name: "VerseTr",
                table: "Verse",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "VerseDesc",
                table: "Verse",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<bool>(
                name: "VerseCheck",
                table: "Verse",
                type: "INTEGER",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "VerseArabic",
                table: "Verse",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "SureId",
                table: "Verse",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Verse",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<bool>(
                name: "RemiderCheck",
                table: "Verse",
                type: "INTEGER",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "RelativeDesk",
                table: "Verse",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "MarkCheck",
                table: "Verse",
                type: "INTEGER",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_updateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 24, 21, 23, 38, 463, DateTimeKind.Local).AddTicks(6156),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 24, 22, 33, 27, 513, DateTimeKind.Local).AddTicks(6191));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_createDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 24, 21, 23, 38, 463, DateTimeKind.Local).AddTicks(6009),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 24, 22, 33, 27, 513, DateTimeKind.Local).AddTicks(6052));

            migrationBuilder.AlterColumn<int>(
                name: "UserLastRelativeVerse",
                table: "Sure",
                type: "INTEGER",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "UserCheckCount",
                table: "Sure",
                type: "INTEGER",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Sure",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfVerses",
                table: "Sure",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Sure",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "LandingLocation",
                table: "Sure",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "DeskMushaf",
                table: "Sure",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "DeskList",
                table: "Sure",
                type: "INTEGER",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "DeskLanding",
                table: "Sure",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Sure",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<bool>(
                name: "Complated",
                table: "Sure",
                type: "INTEGER",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "SendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2022, 11, 24, 21, 23, 38, 464, DateTimeKind.Local).AddTicks(553),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2022, 11, 24, 22, 33, 27, 514, DateTimeKind.Local).AddTicks(470));
        }
    }
}
