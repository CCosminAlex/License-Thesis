using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ADL_Tracker.Migrations
{
    public partial class EditRecommendation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_recommendation",
                table: "recommendation");

            migrationBuilder.DropColumn(
                name: "RecommendationId",
                table: "recommendation");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "recommendation");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "recommendation",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeviated",
                table: "recommendation",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "QuestionnaireScore",
                table: "recommendation",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "SleepHours",
                table: "recommendation",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "TakeMedication",
                table: "recommendation",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_recommendation",
                table: "recommendation",
                column: "Date");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_recommendation",
                table: "recommendation");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "recommendation");

            migrationBuilder.DropColumn(
                name: "IsDeviated",
                table: "recommendation");

            migrationBuilder.DropColumn(
                name: "QuestionnaireScore",
                table: "recommendation");

            migrationBuilder.DropColumn(
                name: "SleepHours",
                table: "recommendation");

            migrationBuilder.DropColumn(
                name: "TakeMedication",
                table: "recommendation");

            migrationBuilder.AddColumn<string>(
                name: "RecommendationId",
                table: "recommendation",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DoctorId",
                table: "recommendation",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_recommendation",
                table: "recommendation",
                column: "RecommendationId");
        }
    }
}
