using Microsoft.EntityFrameworkCore.Migrations;

namespace RetroPieRomUploader.Migrations
{
    public partial class RomFilename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Filename",
                table: "Rom",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Filename",
                table: "Rom");
        }
    }
}
