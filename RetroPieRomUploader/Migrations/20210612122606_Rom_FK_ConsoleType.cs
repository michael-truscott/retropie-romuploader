using Microsoft.EntityFrameworkCore.Migrations;

namespace RetroPieRomUploader.Migrations
{
    public partial class Rom_FK_ConsoleType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Console",
                table: "Rom");

            migrationBuilder.AddColumn<int>(
                name: "ConsoleTypeID",
                table: "Rom",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rom_ConsoleTypeID",
                table: "Rom",
                column: "ConsoleTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Rom_ConsoleType_ConsoleTypeID",
                table: "Rom",
                column: "ConsoleTypeID",
                principalTable: "ConsoleType",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rom_ConsoleType_ConsoleTypeID",
                table: "Rom");

            migrationBuilder.DropIndex(
                name: "IX_Rom_ConsoleTypeID",
                table: "Rom");

            migrationBuilder.DropColumn(
                name: "ConsoleTypeID",
                table: "Rom");

            migrationBuilder.AddColumn<string>(
                name: "Console",
                table: "Rom",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
