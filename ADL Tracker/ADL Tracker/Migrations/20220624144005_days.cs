using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ADL_Tracker.Migrations
{
    public partial class days : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestingDayss",
                columns: table => new
                {
                    TestingDaysId = table.Column<string>(type: "text", nullable: false),
                    Start_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    End_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Activity = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestingDayss", x => x.TestingDaysId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestingDayss");
        }
    }
}
