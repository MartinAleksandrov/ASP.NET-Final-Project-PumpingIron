using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pumping_Iron.Data.Data.Migrations
{
    public partial class MakingClientTrainerDietAndMembershipNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Diets_DietId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Trainers_TrainerId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Clients_TrainingPrograms_TrainingProgramId",
                table: "Clients");

            migrationBuilder.AlterColumn<int>(
                name: "TrainingProgramId",
                table: "Clients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "TrainerId",
                table: "Clients",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "DietId",
                table: "Clients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Diets_DietId",
                table: "Clients",
                column: "DietId",
                principalTable: "Diets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Trainers_TrainerId",
                table: "Clients",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "TrainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_TrainingPrograms_TrainingProgramId",
                table: "Clients",
                column: "TrainingProgramId",
                principalTable: "TrainingPrograms",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Diets_DietId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Trainers_TrainerId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Clients_TrainingPrograms_TrainingProgramId",
                table: "Clients");

            migrationBuilder.AlterColumn<int>(
                name: "TrainingProgramId",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TrainerId",
                table: "Clients",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DietId",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Diets_DietId",
                table: "Clients",
                column: "DietId",
                principalTable: "Diets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Trainers_TrainerId",
                table: "Clients",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "TrainerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_TrainingPrograms_TrainingProgramId",
                table: "Clients",
                column: "TrainingProgramId",
                principalTable: "TrainingPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
