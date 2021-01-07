using Microsoft.EntityFrameworkCore.Migrations;

namespace site.Migrations
{
    public partial class FlashDeals_Schemas_StoreId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "FlashDealProducts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FlashDealProducts_StoreId",
                table: "FlashDealProducts",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_FlashDealProducts_Stores_StoreId",
                table: "FlashDealProducts",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlashDealProducts_Stores_StoreId",
                table: "FlashDealProducts");

            migrationBuilder.DropIndex(
                name: "IX_FlashDealProducts_StoreId",
                table: "FlashDealProducts");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "FlashDealProducts");
        }
    }
}
