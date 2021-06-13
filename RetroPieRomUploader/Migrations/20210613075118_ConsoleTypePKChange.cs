using Microsoft.EntityFrameworkCore.Migrations;

namespace RetroPieRomUploader.Migrations
{
    public partial class ConsoleTypePKChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DirectoryName",
                table: "ConsoleType");

            migrationBuilder.AlterColumn<string>(
                name: "ConsoleTypeID",
                table: "Rom",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "ID",
                table: "ConsoleType",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ConsoleTypeID",
                table: "Rom",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "ConsoleType",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<string>(
                name: "DirectoryName",
                table: "ConsoleType",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
