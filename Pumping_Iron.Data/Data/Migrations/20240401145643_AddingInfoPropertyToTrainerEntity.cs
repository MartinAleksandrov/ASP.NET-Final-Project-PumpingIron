using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pumping_Iron.Data.Data.Migrations
{
    public partial class AddingInfoPropertyToTrainerEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Information",
                table: "Trainers",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Information",
                table: "Trainers");
        }
    }
}
