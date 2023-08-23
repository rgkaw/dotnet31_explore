using Microsoft.EntityFrameworkCore.Migrations;

namespace dotnet31_explore.Migrations
{
    public partial class addOwnedBooks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OwnedBook_Book_BookId",
                table: "OwnedBook");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnedBook_User_Username",
                table: "OwnedBook");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OwnedBook",
                table: "OwnedBook");

            migrationBuilder.RenameTable(
                name: "OwnedBook",
                newName: "OwnedBooks");

            migrationBuilder.RenameIndex(
                name: "IX_OwnedBook_Username",
                table: "OwnedBooks",
                newName: "IX_OwnedBooks_Username");

            migrationBuilder.RenameIndex(
                name: "IX_OwnedBook_BookId",
                table: "OwnedBooks",
                newName: "IX_OwnedBooks_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OwnedBooks",
                table: "OwnedBooks",
                column: "Guid");

            migrationBuilder.AddForeignKey(
                name: "FK_OwnedBooks_Book_BookId",
                table: "OwnedBooks",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OwnedBooks_User_Username",
                table: "OwnedBooks",
                column: "Username",
                principalTable: "User",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OwnedBooks_Book_BookId",
                table: "OwnedBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnedBooks_User_Username",
                table: "OwnedBooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OwnedBooks",
                table: "OwnedBooks");

            migrationBuilder.RenameTable(
                name: "OwnedBooks",
                newName: "OwnedBook");

            migrationBuilder.RenameIndex(
                name: "IX_OwnedBooks_Username",
                table: "OwnedBook",
                newName: "IX_OwnedBook_Username");

            migrationBuilder.RenameIndex(
                name: "IX_OwnedBooks_BookId",
                table: "OwnedBook",
                newName: "IX_OwnedBook_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OwnedBook",
                table: "OwnedBook",
                column: "Guid");

            migrationBuilder.AddForeignKey(
                name: "FK_OwnedBook_Book_BookId",
                table: "OwnedBook",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OwnedBook_User_Username",
                table: "OwnedBook",
                column: "Username",
                principalTable: "User",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
