using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    /// <inheritdoc />
    public partial class MainBuildv100 : Migration
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
                name: "Notes",
                columns: table => new
                {
                    notesId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    verseId = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    sureId = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    subjectId = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    sectionId = table.Column<int>(type: "INTEGER", nullable: false),
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
                name: "Sections",
                columns: table => new
                {
                    SectionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SureId = table.Column<int>(type: "INTEGER", nullable: false),
                    startVerse = table.Column<int>(type: "INTEGER", nullable: false),
                    endVerse = table.Column<int>(type: "INTEGER", nullable: false),
                    SectionName = table.Column<string>(type: "TEXT", nullable: false),
                    SectionDescription = table.Column<string>(type: "TEXT", nullable: false),
                    SectionDetail = table.Column<string>(type: "TEXT", nullable: false),
                    IsMark = table.Column<bool>(type: "INTEGER", nullable: false),
                    SectionNumber = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.SectionId);
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
                    numberOfSection = table.Column<int>(type: "INTEGER", nullable: false),
                    userCheckCount = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    userLastRelativeVerse = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    landingLocation = table.Column<string>(type: "TEXT", nullable: false),
                    deskLanding = table.Column<int>(type: "INTEGER", nullable: false),
                    deskMushaf = table.Column<int>(type: "INTEGER", nullable: false),
                    deskList = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    status = table.Column<string>(type: "TEXT", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: false),
                    completed = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
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
                    user_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user_email = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "none"),
                    user_firstName = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "First Name"),
                    user_lastName = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "Last Name"),
                    pin = table.Column<string>(type: "TEXT", nullable: false),
                    user_screetQuestion = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "Değiştiriniz"),
                    user_screetAnw = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "Yeni Değeri Girin"),
                    user_createDate = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValue: new DateTime(2023, 3, 8, 15, 24, 47, 453, DateTimeKind.Local).AddTicks(563)),
                    user_updateDate = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValue: new DateTime(2023, 3, 8, 15, 24, 47, 453, DateTimeKind.Local).AddTicks(712)),
                    user_avatarUrl = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "default")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.user_id);
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
                    verseCheck = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    markCheck = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    remiderCheck = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verse", x => x.verseId);
                });

            migrationBuilder.CreateTable(
                name: "VerseClass",
                columns: table => new
                {
                    verseClassId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    sureId = table.Column<int>(type: "INTEGER", nullable: false),
                    relativeDesk = table.Column<int>(type: "INTEGER", nullable: false),
                    v_hk = table.Column<bool>(type: "INTEGER", nullable: false),
                    v_tv = table.Column<bool>(type: "INTEGER", nullable: false),
                    v_cz = table.Column<bool>(type: "INTEGER", nullable: false),
                    v_mk = table.Column<bool>(type: "INTEGER", nullable: false),
                    v_du = table.Column<bool>(type: "INTEGER", nullable: false),
                    v_hr = table.Column<bool>(type: "INTEGER", nullable: false),
                    v_sn = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerseClass", x => x.verseClassId);
                });

            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    wordsId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    sureId = table.Column<int>(type: "INTEGER", nullable: true),
                    verseId = table.Column<int>(type: "INTEGER", nullable: true),
                    arp_read = table.Column<string>(type: "TEXT", nullable: true),
                    tr_read = table.Column<string>(type: "TEXT", nullable: true),
                    word_meal = table.Column<string>(type: "TEXT", nullable: true),
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
                name: "Notes");

            migrationBuilder.DropTable(
                name: "Remider");

            migrationBuilder.DropTable(
                name: "Sections");

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
                name: "VerseClass");

            migrationBuilder.DropTable(
                name: "Words");
        }
    }
}
