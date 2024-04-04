using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pumping_Iron.Data.Data.Migrations
{
    public partial class AddOwnerToEveryTrainingProgram : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TrainerId",
                table: "TrainingPrograms",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPrograms_TrainerId",
                table: "TrainingPrograms",
                column: "TrainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingPrograms_Trainers_TrainerId",
                table: "TrainingPrograms",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "TrainerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingPrograms_Trainers_TrainerId",
                table: "TrainingPrograms");

            migrationBuilder.DropIndex(
                name: "IX_TrainingPrograms_TrainerId",
                table: "TrainingPrograms");

            migrationBuilder.DropColumn(
                name: "TrainerId",
                table: "TrainingPrograms");
        }
    }
}
