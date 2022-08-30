using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    public partial class TestV8Hofix3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "VerseCheck",
                table: "Verse",
                type: "TEXT",
                nullable: true,
                defaultValue: "false",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarkCheck",
                table: "Verse",
                type: "TEXT",
                nullable: true,
                defaultValue: "false");

            migrationBuilder.AddColumn<string>(
                name: "RememberCheck",
                table: "Verse",
                type: "TEXT",
                nullable: true,
                defaultValue: "false");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarkCheck",
                table: "Verse");

            migrationBuilder.DropColumn(
                name: "RememberCheck",
                table: "Verse");

            migrationBuilder.AlterColumn<string>(
                name: "VerseCheck",
                table: "Verse",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: "false");
        }
    }
}
