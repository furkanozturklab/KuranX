using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuranX.App.Migrations
{
    public partial class TestV6HotfixColumNam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InterityNote",
                table: "Integrity");

            migrationBuilder.RenameColumn(
                name: "IntergrityName",
                table: "Integrity",
                newName: "IntegrityNote");

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

            migrationBuilder.AddColumn<string>(
                name: "IntegrityName",
                table: "Integrity",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IntegrityName",
                table: "Integrity");

            migrationBuilder.RenameColumn(
                name: "IntegrityNote",
                table: "Integrity",
                newName: "IntergrityName");

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

            migrationBuilder.AddColumn<string>(
                name: "InterityNote",
                table: "Integrity",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
