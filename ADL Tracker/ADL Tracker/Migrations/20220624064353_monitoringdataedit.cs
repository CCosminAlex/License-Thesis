using Microsoft.EntityFrameworkCore.Migrations;

namespace ADL_Tracker.Migrations
{
    public partial class monitoringdataedit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "MonitoringDatas");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Duration",
                table: "MonitoringDatas",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
