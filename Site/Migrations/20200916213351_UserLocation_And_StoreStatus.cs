using Microsoft.EntityFrameworkCore.Migrations;

namespace site.Migrations
{
    public partial class UserLocation_And_StoreStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Stores",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "SiteUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SiteUsers_CampusId",
                table: "SiteUsers",
                column: "CampusId");

            migrationBuilder.AddForeignKey(
                name: "FK_SiteUsers_Campuses_CampusId",
                table: "SiteUsers",
                column: "CampusId",
                principalTable: "Campuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SiteUsers_Campuses_CampusId",
                table: "SiteUsers");

            migrationBuilder.DropIndex(
                name: "IX_SiteUsers_CampusId",
                table: "SiteUsers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "SiteUsers");
        }
    }
}
