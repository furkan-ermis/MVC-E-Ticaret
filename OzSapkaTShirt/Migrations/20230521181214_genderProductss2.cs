using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OzSapkaTShirt.Migrations
{
    public partial class genderProductss2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_GenderProducts_GenderTypesGenderId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_GenderTypesGenderId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "GenderTypesGenderId",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_Products_GenderProduct",
                table: "Products",
                column: "GenderProduct");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_GenderProducts_GenderProduct",
                table: "Products",
                column: "GenderProduct",
                principalTable: "GenderProducts",
                principalColumn: "GenderId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_GenderProducts_GenderProduct",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_GenderProduct",
                table: "Products");

            migrationBuilder.AddColumn<byte>(
                name: "GenderTypesGenderId",
                table: "Products",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_GenderTypesGenderId",
                table: "Products",
                column: "GenderTypesGenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_GenderProducts_GenderTypesGenderId",
                table: "Products",
                column: "GenderTypesGenderId",
                principalTable: "GenderProducts",
                principalColumn: "GenderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
