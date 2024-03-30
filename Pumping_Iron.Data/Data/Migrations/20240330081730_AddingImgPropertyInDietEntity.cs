using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pumping_Iron.Data.Data.Migrations
{
    public partial class AddingImgPropertyInDietEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Diets",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Diets");
        }
    }
}
