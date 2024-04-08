using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pumping_Iron.Data.Data.Migrations
{
    public partial class AddOwnerPropertyToDietEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TrainerId",
                table: "Diets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Diets_TrainerId",
                table: "Diets",
                column: "TrainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Diets_Trainers_TrainerId",
                table: "Diets",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "TrainerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diets_Trainers_TrainerId",
                table: "Diets");

            migrationBuilder.DropIndex(
                name: "IX_Diets_TrainerId",
                table: "Diets");

            migrationBuilder.DropColumn(
                name: "TrainerId",
                table: "Diets");
        }
    }
}
