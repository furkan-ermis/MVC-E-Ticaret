using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OzSapkaTShirt.Migrations
{
    public partial class genderProductss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_GenderProducts_GenderTypeId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "GenderTypeId",
                table: "Products",
                newName: "GenderTypesGenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_GenderTypeId",
                table: "Products",
                newName: "IX_Products_GenderTypesGenderId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "GenderProducts",
                newName: "GenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_GenderProducts_GenderTypesGenderId",
                table: "Products",
                column: "GenderTypesGenderId",
                principalTable: "GenderProducts",
                principalColumn: "GenderId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_GenderProducts_GenderTypesGenderId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "GenderTypesGenderId",
                table: "Products",
                newName: "GenderTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_GenderTypesGenderId",
                table: "Products",
                newName: "IX_Products_GenderTypeId");

            migrationBuilder.RenameColumn(
                name: "GenderId",
                table: "GenderProducts",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_GenderProducts_GenderTypeId",
                table: "Products",
                column: "GenderTypeId",
                principalTable: "GenderProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
