using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace site.Migrations
{
    public partial class Store_Subscriptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDocumentVerified",
                table: "Stores",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSubscribedOn",
                table: "Stores",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LastSubscriptionAmount",
                table: "Stores",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubscriptionExpiresOn",
                table: "Stores",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDocumentVerified",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "LastSubscribedOn",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "LastSubscriptionAmount",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "SubscriptionExpiresOn",
                table: "Stores");
        }
    }
}
