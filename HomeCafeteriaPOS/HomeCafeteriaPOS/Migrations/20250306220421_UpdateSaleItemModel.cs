using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeCafeteriaPOS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSaleItemModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SaleItem_ProductId",
                table: "SaleItem",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleItem_Products_ProductId",
                table: "SaleItem",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleItem_Products_ProductId",
                table: "SaleItem");

            migrationBuilder.DropIndex(
                name: "IX_SaleItem_ProductId",
                table: "SaleItem");
        }
    }
}
