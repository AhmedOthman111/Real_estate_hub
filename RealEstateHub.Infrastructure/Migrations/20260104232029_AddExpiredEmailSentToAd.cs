using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExpiredEmailSentToAd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ExpiredEmailSent",
                table: "Ads",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiredEmailSent",
                table: "Ads");
        }
    }
}
