using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    public partial class MainBuild_2Hotfix1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ResultVerse",
                table: "Results",
                type: "TEXT",
                nullable: false,
                defaultValue: "false",
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "ResultSubject",
                table: "Results",
                type: "TEXT",
                nullable: false,
                defaultValue: "false",
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "ResultNotes",
                table: "Results",
                type: "TEXT",
                nullable: false,
                defaultValue: "false",
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "ResultLib",
                table: "Results",
                type: "TEXT",
                nullable: false,
                defaultValue: "false",
                oldClrType: typeof(string),
                oldType: "TEXT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ResultVerse",
                table: "Results",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "false");

            migrationBuilder.AlterColumn<string>(
                name: "ResultSubject",
                table: "Results",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "false");

            migrationBuilder.AlterColumn<string>(
                name: "ResultNotes",
                table: "Results",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "false");

            migrationBuilder.AlterColumn<string>(
                name: "ResultLib",
                table: "Results",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "false");
        }
    }
}
