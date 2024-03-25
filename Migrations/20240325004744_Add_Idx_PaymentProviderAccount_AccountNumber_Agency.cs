using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PixHub.Migrations
{
    /// <inheritdoc />
    public partial class Add_Idx_PaymentProviderAccount_AccountNumber_Agency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PaymentProviderAccount_AccountNumber_Agency",
                table: "PaymentProviderAccount",
                columns: new[] { "AccountNumber", "Agency" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PaymentProviderAccount_AccountNumber_Agency",
                table: "PaymentProviderAccount");
        }
    }
}
