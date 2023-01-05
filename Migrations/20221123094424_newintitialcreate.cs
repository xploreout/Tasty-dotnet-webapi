using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeApi.Migrations
{
    public partial class newintitialcreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeUser_Recipe_RecipesId",
                table: "RecipeUser");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeUser_User_UsersId",
                table: "RecipeUser");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "RecipeUser",
                newName: "RecipeId");

            migrationBuilder.RenameColumn(
                name: "RecipesId",
                table: "RecipeUser",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeUser_UsersId",
                table: "RecipeUser",
                newName: "IX_RecipeUser_RecipeId");

            migrationBuilder.AlterColumn<int>(
                name: "RecipeId",
                table: "RecipeUser",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "RecipeUser",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeUser_Recipe_RecipeId",
                table: "RecipeUser",
                column: "RecipeId",
                principalTable: "Recipe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeUser_User_UserId",
                table: "RecipeUser",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeUser_Recipe_RecipeId",
                table: "RecipeUser");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeUser_User_UserId",
                table: "RecipeUser");

            migrationBuilder.RenameColumn(
                name: "RecipeId",
                table: "RecipeUser",
                newName: "UsersId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "RecipeUser",
                newName: "RecipesId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeUser_RecipeId",
                table: "RecipeUser",
                newName: "IX_RecipeUser_UsersId");

            migrationBuilder.AlterColumn<int>(
                name: "UsersId",
                table: "RecipeUser",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "RecipesId",
                table: "RecipeUser",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeUser_Recipe_RecipesId",
                table: "RecipeUser",
                column: "RecipesId",
                principalTable: "Recipe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeUser_User_UsersId",
                table: "RecipeUser",
                column: "UsersId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
