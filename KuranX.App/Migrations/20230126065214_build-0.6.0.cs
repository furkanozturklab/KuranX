using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    /// <inheritdoc />
    public partial class build060 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.DropColumn(
                name: "resultLib",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "libraryId",
                table: "Notes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_updateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 26, 9, 52, 14, 254, DateTimeKind.Local).AddTicks(4554),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 1, 10, 20, 54, 35, 891, DateTimeKind.Local).AddTicks(417));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_createDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 26, 9, 52, 14, 254, DateTimeKind.Local).AddTicks(4380),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 1, 10, 20, 54, 35, 891, DateTimeKind.Local).AddTicks(218));

            migrationBuilder.AlterColumn<DateTime>(
                name: "sendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 26, 9, 52, 14, 254, DateTimeKind.Local).AddTicks(8399),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 1, 10, 20, 54, 35, 891, DateTimeKind.Local).AddTicks(4895));

            migrationBuilder.AddColumn<int>(
                name: "sectionId",
                table: "Notes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropColumn(
                name: "sectionId",
                table: "Notes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_updateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 10, 20, 54, 35, 891, DateTimeKind.Local).AddTicks(417),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 1, 26, 9, 52, 14, 254, DateTimeKind.Local).AddTicks(4554));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_createDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 10, 20, 54, 35, 891, DateTimeKind.Local).AddTicks(218),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 1, 26, 9, 52, 14, 254, DateTimeKind.Local).AddTicks(4380));

            migrationBuilder.AddColumn<bool>(
                name: "resultLib",
                table: "Results",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "sendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 10, 20, 54, 35, 891, DateTimeKind.Local).AddTicks(4895),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 1, 26, 9, 52, 14, 254, DateTimeKind.Local).AddTicks(8399));

            migrationBuilder.AddColumn<int>(
                name: "libraryId",
                table: "Notes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Librarys",
                columns: table => new
                {
                    libraryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    libraryColor = table.Column<string>(type: "TEXT", nullable: false),
                    libraryName = table.Column<string>(type: "TEXT", nullable: false),
                    modify = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Librarys", x => x.libraryId);
                });
        }
    }
}
