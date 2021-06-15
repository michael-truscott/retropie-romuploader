using Microsoft.EntityFrameworkCore.Migrations;

namespace RetroPieRomUploader.Migrations
{
    public partial class RomMultipleFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RomFileEntry",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RomID = table.Column<int>(type: "INTEGER", nullable: false),
                    Filename = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RomFileEntry", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RomFileEntry_Rom_RomID",
                        column: x => x.RomID,
                        principalTable: "Rom",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RomFileEntry_RomID",
                table: "RomFileEntry",
                column: "RomID");

            migrationBuilder.Sql("INSERT INTO RomFileEntry (RomID, Filename)" +
                                "SELECT ID, Filename FROM Rom");

            migrationBuilder.DropColumn(
                name: "Filename",
                table: "Rom");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RomFileEntry");

            migrationBuilder.AddColumn<string>(
                name: "Filename",
                table: "Rom",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
