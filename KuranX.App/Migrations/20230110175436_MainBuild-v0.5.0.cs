using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    /// <inheritdoc />
    public partial class MainBuildv050 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "commentary",
                table: "Verse");

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_updateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 10, 20, 54, 35, 891, DateTimeKind.Local).AddTicks(417),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 1, 9, 19, 4, 56, 575, DateTimeKind.Local).AddTicks(5099));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_createDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 10, 20, 54, 35, 891, DateTimeKind.Local).AddTicks(218),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 1, 9, 19, 4, 56, 575, DateTimeKind.Local).AddTicks(4907));

            migrationBuilder.AlterColumn<DateTime>(
                name: "sendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 10, 20, 54, 35, 891, DateTimeKind.Local).AddTicks(4895),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 1, 9, 19, 4, 56, 575, DateTimeKind.Local).AddTicks(9540));

            migrationBuilder.CreateTable(
                name: "VerseClass",
                columns: table => new
                {
                    verseClassId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    sureId = table.Column<int>(type: "INTEGER", nullable: false),
                    relativeDesk = table.Column<int>(type: "INTEGER", nullable: false),
                    vhk = table.Column<bool>(name: "v_hk", type: "INTEGER", nullable: false),
                    vtv = table.Column<bool>(name: "v_tv", type: "INTEGER", nullable: false),
                    vcz = table.Column<bool>(name: "v_cz", type: "INTEGER", nullable: false),
                    vmk = table.Column<bool>(name: "v_mk", type: "INTEGER", nullable: false),
                    vdu = table.Column<bool>(name: "v_du", type: "INTEGER", nullable: false),
                    vhr = table.Column<bool>(name: "v_hr", type: "INTEGER", nullable: false),
                    vsn = table.Column<bool>(name: "v_sn", type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerseClass", x => x.verseClassId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VerseClass");

            migrationBuilder.AddColumn<string>(
                name: "commentary",
                table: "Verse",
                type: "TEXT",
                nullable: false,
                defaultValue: "Wait");

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_updateDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 9, 19, 4, 56, 575, DateTimeKind.Local).AddTicks(5099),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 1, 10, 20, 54, 35, 891, DateTimeKind.Local).AddTicks(417));

            migrationBuilder.AlterColumn<DateTime>(
                name: "user_createDate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 9, 19, 4, 56, 575, DateTimeKind.Local).AddTicks(4907),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 1, 10, 20, 54, 35, 891, DateTimeKind.Local).AddTicks(218));

            migrationBuilder.AlterColumn<DateTime>(
                name: "sendTime",
                table: "ResultItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 9, 19, 4, 56, 575, DateTimeKind.Local).AddTicks(9540),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValue: new DateTime(2023, 1, 10, 20, 54, 35, 891, DateTimeKind.Local).AddTicks(4895));
        }
    }
}
