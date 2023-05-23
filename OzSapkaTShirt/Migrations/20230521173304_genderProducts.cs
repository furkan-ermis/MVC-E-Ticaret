using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OzSapkaTShirt.Migrations
{
    public partial class genderProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Products");

            migrationBuilder.AddColumn<byte>(
                name: "GenderProduct",
                table: "Products",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "GenderTypeId",
                table: "Products",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateTable(
                name: "GenderProducts",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false),
                    Name = table.Column<string>(type: "nchar(15)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenderProducts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_GenderTypeId",
                table: "Products",
                column: "GenderTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_GenderProducts_GenderTypeId",
                table: "Products",
                column: "GenderTypeId",
                principalTable: "GenderProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_GenderProducts_GenderTypeId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "GenderProducts");

            migrationBuilder.DropIndex(
                name: "IX_Products_GenderTypeId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "GenderProduct",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "GenderTypeId",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
