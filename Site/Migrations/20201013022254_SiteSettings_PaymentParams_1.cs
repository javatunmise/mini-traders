using Microsoft.EntityFrameworkCore.Migrations;

namespace site.Migrations
{
    public partial class SiteSettings_PaymentParams_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SignOnFee",
                table: "Sites",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WithdrawalCharge",
                table: "Sites",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SignOnFee",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "WithdrawalCharge",
                table: "Sites");
        }
    }
}
