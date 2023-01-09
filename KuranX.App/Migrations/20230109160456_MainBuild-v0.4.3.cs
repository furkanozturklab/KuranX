using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    /// <inheritdoc />
    public partial class MainBuildv043 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Integrity",
                columns: table => new
                {
                    integrityId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    integrityName = table.Column<string>(type: "TEXT", nullable: false),
                    connectVerseId = table.Column<int>(type: "INTEGER", nullable: false),
                    connectSureId = table.Column<int>(type: "INTEGER", nullable: false),
                    connectedVerseId = table.Column<int>(type: "INTEGER", nullable: false),
                    connectedSureId = table.Column<int>(type: "INTEGER", nullable: false),
                    integrityNote = table.Column<string>(type: "TEXT", nullable: false),
                    integrityProtected = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    modify = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Integrity", x => x.integrityId);
                });

            migrationBuilder.CreateTable(
                name: "Interpreter",
                columns: table => new
                {
                    interpreterId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    verseId = table.Column<int>(type: "INTEGER", nullable: false),
                    sureId = table.Column<int>(type: "INTEGER", nullable: false),
                    interpreterWriter = table.Column<string>(type: "TEXT", nullable: false),
                    interpreterDetail = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interpreter", x => x.interpreterId);
                });

            migrationBuilder.CreateTable(
                name: "Librarys",
                columns: table => new
                {
                    libraryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    libraryName = table.Column<string>(type: "TEXT", nullable: false),
                    libraryColor = table.Column<string>(type: "TEXT", nullable: false),
                    created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    modify = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Librarys", x => x.libraryId);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    notesId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    verseId = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    sureId = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    subjectId = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    libraryId = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    noteHeader = table.Column<string>(type: "TEXT", nullable: false),
                    noteDetail = table.Column<string>(type: "TEXT", nullable: false),
                    noteLocation = table.Column<string>(type: "TEXT", nullable: false),
                    created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    modify = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.notesId);
                });

            migrationBuilder.CreateTable(
                name: "Remider",
                columns: table => new
                {
                    remiderId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    remiderName = table.Column<string>(type: "TEXT", nullable: false),
                    remiderDetail = table.Column<string>(type: "TEXT", nullable: false),
                    loopType = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "Default"),
                    status = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "Wait"),
                    remiderDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    create = table.Column<DateTime>(type: "TEXT", nullable: false),
                    lastAction = table.Column<DateTime>(type: "TEXT", nullable: false),
                    connectVerseId = table.Column<int>(type: "INTEGER", nullable: false),
                    connectSureId = table.Column<int>(type: "INTEGER", nullable: false),
                    priority = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remider", x => x.remiderId);
                });

            migrationBuilder.CreateTable(
                name: "ResultItems",
                columns: table => new
                {
                    resultItemId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    resultId = table.Column<int>(type: "INTEGER", nullable: false),
                    resultSubjectId = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    resultLibId = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    resultNoteId = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    sendTime = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValue: new DateTime(2023, 1, 9, 19, 4, 56, 575, DateTimeKind.Local).AddTicks(9540))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultItems", x => x.resultItemId);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    resultId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    resultName = table.Column<string>(type: "TEXT", nullable: false),
                    resultLib = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    resultNotes = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    resultSubject = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    resultFinallyNote = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "Sonuç Metninizi buraya yaza bilirsiniz.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.resultId);
                });

            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    subjectId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    subjectName = table.Column<string>(type: "TEXT", nullable: false),
                    subjectColor = table.Column<string>(type: "TEXT", nullable: false),
                    created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    modify = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.subjectId);
                });

            migrationBuilder.CreateTable(
                name: "SubjectItems",
                columns: table => new
                {
                    subjectItemsId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    subjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    sureId = table.Column<int>(type: "INTEGER", nullable: false),
                    verseId = table.Column<int>(type: "INTEGER", nullable: false),
                    subjectNotesId = table.Column<int>(type: "INTEGER", nullable: false),
                    subjectName = table.Column<string>(type: "TEXT", nullable: false),
                    created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    modify = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectItems", x => x.subjectItemsId);
                });

            migrationBuilder.CreateTable(
                name: "Sure",
                columns: table => new
                {
                    sureId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    numberOfVerses = table.Column<int>(type: "INTEGER", nullable: false),
                    userCheckCount = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    userLastRelativeVerse = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    landingLocation = table.Column<string>(type: "TEXT", nullable: false),
                    deskLanding = table.Column<int>(type: "INTEGER", nullable: false),
                    deskMushaf = table.Column<int>(type: "INTEGER", nullable: false),
                    deskList = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    status = table.Column<string>(type: "TEXT", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: false),
                    complated = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sure", x => x.sureId);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    tasksId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    missonsId = table.Column<int>(type: "INTEGER", nullable: false),
                    missonsTime = table.Column<int>(type: "INTEGER", nullable: false),
                    missonsRepeart = table.Column<int>(type: "INTEGER", nullable: false),
                    missonsType = table.Column<string>(type: "TEXT", nullable: false),
                    missonsColor = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.tasksId);
                });

            migrationBuilder.CreateTable(
                name: "UserHelp",
                columns: table => new
                {
                    UserhelpId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    baseName = table.Column<string>(type: "TEXT", nullable: false),
                    infoName = table.Column<string>(type: "TEXT", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: false),
                    infoImage = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHelp", x => x.UserhelpId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userid = table.Column<int>(name: "user_id", type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    useremail = table.Column<string>(name: "user_email", type: "TEXT", nullable: false, defaultValue: "none"),
                    userfirstName = table.Column<string>(name: "user_firstName", type: "TEXT", nullable: false, defaultValue: "First Name"),
                    userlastName = table.Column<string>(name: "user_lastName", type: "TEXT", nullable: false, defaultValue: "Last Name"),
                    pin = table.Column<string>(type: "TEXT", nullable: false),
                    userscreetQuestion = table.Column<string>(name: "user_screetQuestion", type: "TEXT", nullable: false, defaultValue: "Değiştiriniz"),
                    userscreetAnw = table.Column<string>(name: "user_screetAnw", type: "TEXT", nullable: false, defaultValue: "Yeni Değeri Girin"),
                    usercreateDate = table.Column<DateTime>(name: "user_createDate", type: "TEXT", nullable: false, defaultValue: new DateTime(2023, 1, 9, 19, 4, 56, 575, DateTimeKind.Local).AddTicks(4907)),
                    userupdateDate = table.Column<DateTime>(name: "user_updateDate", type: "TEXT", nullable: false, defaultValue: new DateTime(2023, 1, 9, 19, 4, 56, 575, DateTimeKind.Local).AddTicks(5099)),
                    useravatarUrl = table.Column<string>(name: "user_avatarUrl", type: "TEXT", nullable: false, defaultValue: "default")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userid);
                });

            migrationBuilder.CreateTable(
                name: "Verse",
                columns: table => new
                {
                    verseId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    sureId = table.Column<int>(type: "INTEGER", nullable: false),
                    relativeDesk = table.Column<int>(type: "INTEGER", nullable: false),
                    verseArabic = table.Column<string>(type: "TEXT", nullable: false),
                    verseTr = table.Column<string>(type: "TEXT", nullable: false),
                    verseDesc = table.Column<string>(type: "TEXT", nullable: false),
                    commentary = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "Wait"),
                    verseCheck = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    markCheck = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    remiderCheck = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verse", x => x.verseId);
                });

            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    wordsId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    sureId = table.Column<int>(type: "INTEGER", nullable: true),
                    verseId = table.Column<int>(type: "INTEGER", nullable: true),
                    arpread = table.Column<string>(name: "arp_read", type: "TEXT", nullable: true),
                    trread = table.Column<string>(name: "tr_read", type: "TEXT", nullable: true),
                    wordmeal = table.Column<string>(name: "word_meal", type: "TEXT", nullable: true),
                    root = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.wordsId);
                });
        }

        /// <inheritdoc />
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
                name: "UserHelp");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Verse");

            migrationBuilder.DropTable(
                name: "Words");
        }
    }
}
