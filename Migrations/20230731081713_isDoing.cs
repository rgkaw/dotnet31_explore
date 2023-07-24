using Microsoft.EntityFrameworkCore.Migrations;

namespace mvc.Migrations
{
    public partial class isDoing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isDoing",
                table: "EventsSchedules",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDoing",
                table: "EventsSchedules");
        }
    }
}
