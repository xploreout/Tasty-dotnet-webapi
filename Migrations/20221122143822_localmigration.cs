using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeApi.Migrations
{
    public partial class localmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Steps",
                table: "Recipe",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Recipe",
                newName: "Instructions");

            migrationBuilder.AddColumn<int>(
                name: "CookingTime",
                table: "Recipe",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageAltText",
                table: "Recipe",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nutrition",
                table: "Recipe",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CookingTime",
                table: "Recipe");

            migrationBuilder.DropColumn(
                name: "ImageAltText",
                table: "Recipe");

            migrationBuilder.DropColumn(
                name: "Nutrition",
                table: "Recipe");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Recipe",
                newName: "Steps");

            migrationBuilder.RenameColumn(
                name: "Instructions",
                table: "Recipe",
                newName: "Name");
        }
    }
}
