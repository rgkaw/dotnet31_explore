using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace mvc.Migrations
{
    public partial class Events : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventTypes",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTypes", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    TypeGuid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Events_EventTypes_TypeGuid",
                        column: x => x.TypeGuid,
                        principalTable: "EventTypes",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventsSchedules",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    EventGuid = table.Column<Guid>(nullable: true),
                    Start = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventsSchedules", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_EventsSchedules_Events_EventGuid",
                        column: x => x.EventGuid,
                        principalTable: "Events",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_TypeGuid",
                table: "Events",
                column: "TypeGuid");

            migrationBuilder.CreateIndex(
                name: "IX_EventsSchedules_EventGuid",
                table: "EventsSchedules",
                column: "EventGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventsSchedules");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "EventTypes");
        }
    }
}
