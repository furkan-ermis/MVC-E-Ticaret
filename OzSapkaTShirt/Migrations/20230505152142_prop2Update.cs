using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OzSapkaTShirt.Migrations
{
    public partial class prop2Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                table: "Properties",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_CategoryId",
                table: "Properties",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Categories_CategoryId",
                table: "Properties",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Categories_CategoryId",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_CategoryId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Properties");
        }
    }
}
