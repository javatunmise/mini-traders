using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace site.Migrations
{
    public partial class ProductReviews_Schema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductReviews",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: false),
                    StoreId = table.Column<int>(nullable: false),
                    Message = table.Column<string>(maxLength: 1024, nullable: true),
                    ReviewerId = table.Column<int>(nullable: false),
                    ReviewerName = table.Column<string>(maxLength: 100, nullable: true),
                    Rating = table.Column<double>(nullable: false),
                    HideUserIdentity = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductReviews_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_And_Store",
                table: "ProductReviews",
                columns: new[] { "ProductId", "StoreId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductReviews");
        }
    }
}
