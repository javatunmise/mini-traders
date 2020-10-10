using Microsoft.EntityFrameworkCore.Migrations;

namespace site.Migrations
{
    public partial class SitePayment_ExternalRef : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalRef",
                table: "Payments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalRef",
                table: "Payments");
        }
    }
}
