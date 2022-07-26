﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace ADL_Tracker.Migrations
{
    public partial class EditRecommendationUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "recommendation",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Text",
                table: "recommendation");
        }
    }
}
