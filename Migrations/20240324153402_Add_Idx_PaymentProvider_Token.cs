using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PixHub.Migrations
{
    /// <inheritdoc />
    public partial class Add_Idx_PaymentProvider_Token : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "Idx_PaymentProvider_Token",
                table: "PaymentProvider",
                column: "Token",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Idx_PaymentProvider_Token",
                table: "PaymentProvider");
        }
    }
}
