using Microsoft.EntityFrameworkCore.Migrations;

namespace mvc.Migrations
{
    public partial class addBookColumn1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "stock",
                table: "Book",
                newName: "Stock");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "Book",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "dateCreated",
                table: "Book",
                newName: "DateCreated");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Stock",
                table: "Book",
                newName: "stock");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Book",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Book",
                newName: "dateCreated");
        }
    }
}
