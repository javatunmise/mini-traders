using Microsoft.EntityFrameworkCore.Migrations;

namespace site.Migrations
{
    public partial class WithdrawalRequest_ReversalAndCharges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Charge",
                table: "WithdrawRequests",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Charge",
                table: "WithdrawRequests");
        }
    }
}
