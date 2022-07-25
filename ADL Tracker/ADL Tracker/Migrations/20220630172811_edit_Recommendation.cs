using Microsoft.EntityFrameworkCore.Migrations;

namespace ADL_Tracker.Migrations
{
    public partial class edit_Recommendation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Deviation",
                table: "recommendation",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deviation",
                table: "recommendation");
        }
    }
}
