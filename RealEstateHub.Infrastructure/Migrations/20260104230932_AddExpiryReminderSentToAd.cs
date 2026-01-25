using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExpiryReminderSentToAd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpireAt",
                table: "Ads",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ExpiryReminderSent",
                table: "Ads",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiryReminderSent",
                table: "Ads");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpireAt",
                table: "Ads",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
