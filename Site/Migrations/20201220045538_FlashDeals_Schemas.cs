using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace site.Migrations
{
    public partial class FlashDeals_Schemas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlashDeals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    MinimumDiscount = table.Column<decimal>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashDeals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FlashDealProducts",
                columns: table => new
                {
                    FlashDealId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    OldPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashDealProducts", x => new { x.ProductId, x.FlashDealId });
                    table.ForeignKey(
                        name: "FK_FlashDealProducts_FlashDeals_FlashDealId",
                        column: x => x.FlashDealId,
                        principalTable: "FlashDeals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_FlashDealProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlashDealProducts_FlashDealId",
                table: "FlashDealProducts",
                column: "FlashDealId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlashDealProducts");

            migrationBuilder.DropTable(
                name: "FlashDeals");
        }
    }
}
