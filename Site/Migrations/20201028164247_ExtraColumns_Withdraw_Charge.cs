using Microsoft.EntityFrameworkCore.Migrations;

namespace site.Migrations
{
    public partial class ExtraColumns_Withdraw_Charge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "WithdrawRequests",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "WithdrawRequests",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "WithdrawRequests");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "WithdrawRequests");
        }
    }
}
