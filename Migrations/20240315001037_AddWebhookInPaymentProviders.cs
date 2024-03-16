using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PixHub.Migrations
{
    /// <inheritdoc />
    public partial class AddWebhookInPaymentProviders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Webhook",
                table: "PaymentProvider",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Webhook",
                table: "PaymentProvider");
        }
    }
}
