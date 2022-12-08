using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    public partial class MainBuild : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Integrity",
                columns: table => new
                {
                    IntegrityId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IntegrityName = table.Column<string>(type: "TEXT", nullable: true),
                    connectVerseId = table.Column<int>(type: "INTEGER", nullable: true),
                    connectSureId = table.Column<int>(type: "INTEGER", nullable: true),
                    connectedVerseId = table.Column<int>(type: "INTEGER", nullable: true),
                    connectedSureId = table.Column<int>(type: "INTEGER", nullable: true),
                    IntegrityNote = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modify = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Integrity", x => x.IntegrityId);
                });

            migrationBuilder.CreateTable(
                name: "Interpreter",
                columns: table => new
                {
                    interpreterId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    verseId = table.Column<int>(type: "INTEGER", nullable: true),
                    sureId = table.Column<int>(type: "INTEGER", nullable: true),
                    interpreterWriter = table.Column<string>(type: "TEXT", nullable: true),
                    interpreterDetail = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interpreter", x => x.interpreterId);
                });

            migrationBuilder.CreateTable(
                name: "Librarys",
                columns: table => new
                {
                    LibraryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LibraryName = table.Column<string>(type: "TEXT", nullable: true),
                    LibraryColor = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modify = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Librarys", x => x.LibraryId);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    NotesId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VerseId = table.Column<int>(type: "INTEGER", nullable: true, defaultValue: 0),
                    SureId = table.Column<int>(type: "INTEGER", nullable: true, defaultValue: 0),
                    SubjectId = table.Column<int>(type: "INTEGER", nullable: true, defaultValue: 0),
                    PdfFileId = table.Column<int>(type: "INTEGER", nullable: true, defaultValue: 0),
                    PdfPageId = table.Column<int>(type: "INTEGER", nullable: true, defaultValue: 1),
                    LibraryId = table.Column<int>(type: "INTEGER", nullable: true, defaultValue: 0),
                    NoteHeader = table.Column<string>(type: "TEXT", nullable: true),
                    NoteDetail = table.Column<string>(type: "TEXT", nullable: true),
                    NoteLocation = table.Column<string>(type: "TEXT", nullable: true),
                    NoteStatus = table.Column<string>(type: "TEXT", nullable: true, defaultValue: "#ADB5BD"),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modify = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.NotesId);
                });

            migrationBuilder.CreateTable(
                name: "PdfFile",
                columns: table => new
                {
                    PdfFileId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FileName = table.Column<string>(type: "TEXT", nullable: false),
                    FileUrl = table.Column<string>(type: "TEXT", nullable: false),
                    FileSize = table.Column<string>(type: "TEXT", nullable: false),
                    FileType = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "User"),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modify = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PdfFile", x => x.PdfFileId);
                });

            migrationBuilder.CreateTable(
                name: "Remider",
                columns: table => new
                {
                    RemiderId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RemiderName = table.Column<string>(type: "TEXT", nullable: false),
                    RemiderDetail = table.Column<string>(type: "TEXT", nullable: false),
                    LoopType = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "False"),
                    Status = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "Wait"),
                    RemiderDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Create = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastAction = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ConnectVerseId = table.Column<int>(type: "INTEGER", nullable: false),
                    ConnectSureId = table.Column<int>(type: "INTEGER", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remider", x => x.RemiderId);
                });

            migrationBuilder.CreateTable(
                name: "ResultItems",
                columns: table => new
                {
                    ResultItemId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ResultId = table.Column<int>(type: "INTEGER", nullable: false),
                    ResultSubjectId = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    ResultLibId = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    ResultNoteId = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    SendTime = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValue: new DateTime(2022, 11, 3, 19, 16, 50, 78, DateTimeKind.Local).AddTicks(6339))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultItems", x => x.ResultItemId);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    ResultId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ResultName = table.Column<string>(type: "TEXT", nullable: false),
                    ResultVerse = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "false"),
                    ResultLib = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "false"),
                    ResultNotes = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "false"),
                    ResultSubject = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "false"),
                    ResultStatus = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "#ADB5BD"),
                    ResultFinallyNote = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "Sonuç Metninizi buraya yaza bilirsiniz.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.ResultId);
                });

            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    SubjectId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SubjectName = table.Column<string>(type: "TEXT", nullable: true),
                    SubjectColor = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modify = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.SubjectId);
                });

            migrationBuilder.CreateTable(
                name: "SubjectItems",
                columns: table => new
                {
                    SubjectItemsId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SubjectId = table.Column<int>(type: "INTEGER", nullable: true),
                    SureId = table.Column<int>(type: "INTEGER", nullable: true),
                    VerseId = table.Column<int>(type: "INTEGER", nullable: true),
                    SubjectNotesId = table.Column<int>(type: "INTEGER", nullable: true),
                    SubjectName = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modify = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectItems", x => x.SubjectItemsId);
                });

            migrationBuilder.CreateTable(
                name: "Sure",
                columns: table => new
                {
                    sureId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    NumberOfVerses = table.Column<int>(type: "INTEGER", nullable: true),
                    UserCheckCount = table.Column<int>(type: "INTEGER", nullable: true, defaultValue: 0),
                    UserLastRelativeVerse = table.Column<int>(type: "INTEGER", nullable: true, defaultValue: 1),
                    LandingLocation = table.Column<string>(type: "TEXT", nullable: true),
                    DeskLanding = table.Column<int>(type: "INTEGER", nullable: true),
                    DeskMushaf = table.Column<int>(type: "INTEGER", nullable: true),
                    DeskList = table.Column<int>(type: "INTEGER", nullable: true, defaultValue: 0),
                    Status = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Complated = table.Column<bool>(type: "INTEGER", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sure", x => x.sureId);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    TasksId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MissonsId = table.Column<int>(type: "INTEGER", nullable: false),
                    MissonsTime = table.Column<int>(type: "INTEGER", nullable: false),
                    MissonsRepeart = table.Column<int>(type: "INTEGER", nullable: false),
                    MissonsType = table.Column<string>(type: "TEXT", nullable: false),
                    MissonsColor = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.TasksId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user_email = table.Column<string>(type: "TEXT", nullable: false),
                    user_fisrtName = table.Column<string>(type: "TEXT", nullable: true, defaultValue: "First Name"),
                    user_lastName = table.Column<string>(type: "TEXT", nullable: true, defaultValue: "Last Name"),
                    user_password = table.Column<string>(type: "TEXT", nullable: true),
                    user_screetQuestion = table.Column<string>(type: "TEXT", nullable: true, defaultValue: "Screet Question"),
                    user_createDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    user_updateDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    user_avatarUrl = table.Column<string>(type: "TEXT", nullable: true, defaultValue: "default")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "Verse",
                columns: table => new
                {
                    VerseId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SureId = table.Column<int>(type: "INTEGER", nullable: true),
                    RelativeDesk = table.Column<int>(type: "INTEGER", nullable: true),
                    VerseArabic = table.Column<string>(type: "TEXT", nullable: true),
                    VerseTr = table.Column<string>(type: "TEXT", nullable: true),
                    VerseDesc = table.Column<string>(type: "TEXT", nullable: true),
                    VerseCheck = table.Column<string>(type: "TEXT", nullable: true, defaultValue: "false"),
                    MarkCheck = table.Column<string>(type: "TEXT", nullable: true, defaultValue: "false"),
                    RemiderCheck = table.Column<string>(type: "TEXT", nullable: true, defaultValue: "false"),
                    Status = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verse", x => x.VerseId);
                });

            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    WordsId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VerseId = table.Column<int>(type: "INTEGER", nullable: true),
                    SureId = table.Column<int>(type: "INTEGER", nullable: true),
                    WordText = table.Column<string>(type: "TEXT", nullable: true),
                    WordRe = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.WordsId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Integrity");

            migrationBuilder.DropTable(
                name: "Interpreter");

            migrationBuilder.DropTable(
                name: "Librarys");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "PdfFile");

            migrationBuilder.DropTable(
                name: "Remider");

            migrationBuilder.DropTable(
                name: "ResultItems");

            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "Subject");

            migrationBuilder.DropTable(
                name: "SubjectItems");

            migrationBuilder.DropTable(
                name: "Sure");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Verse");

            migrationBuilder.DropTable(
                name: "Words");
        }
    }
}