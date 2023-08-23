using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace dotnet31_explore.Migrations
{
    public partial class addlistforownedbook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OwnedBook",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(nullable: false),
                    BookId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnedBook", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_OwnedBook_Book_BookId",
                        column: x => x.BookId,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OwnedBook_User_Username",
                        column: x => x.Username,
                        principalTable: "User",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OwnedBook_BookId",
                table: "OwnedBook",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnedBook_Username",
                table: "OwnedBook",
                column: "Username");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OwnedBook");
        }
    }
}
