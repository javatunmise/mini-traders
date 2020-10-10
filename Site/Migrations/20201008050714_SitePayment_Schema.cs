using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace site.Migrations
{
    public partial class SitePayment_Schema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentRef = table.Column<string>(maxLength: 20, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EntityId = table.Column<string>(maxLength: 50, nullable: true),
                    EntityType = table.Column<string>(maxLength: 20, nullable: true),
                    PaymentDescription = table.Column<string>(maxLength: 128, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    PaymentDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Email = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentRef);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");
        }
    }
}
