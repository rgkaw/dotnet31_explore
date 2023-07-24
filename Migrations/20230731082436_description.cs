using Microsoft.EntityFrameworkCore.Migrations;

namespace mvc.Migrations
{
    public partial class description : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isDoing",
                table: "EventsSchedules",
                newName: "IsDoing");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "EventsSchedules",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "EventsSchedules");

            migrationBuilder.RenameColumn(
                name: "IsDoing",
                table: "EventsSchedules",
                newName: "isDoing");
        }
    }
}
