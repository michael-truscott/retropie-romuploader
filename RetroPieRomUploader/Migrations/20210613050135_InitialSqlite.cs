using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RetroPieRomUploader.Migrations
{
    public partial class InitialSqlite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsoleType",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    DirectoryName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsoleType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Rom",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    ConsoleTypeID = table.Column<int>(type: "INTEGER", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Filename = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rom", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Rom_ConsoleType_ConsoleTypeID",
                        column: x => x.ConsoleTypeID,
                        principalTable: "ConsoleType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rom_ConsoleTypeID",
                table: "Rom",
                column: "ConsoleTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rom");

            migrationBuilder.DropTable(
                name: "ConsoleType");
        }
    }
}
