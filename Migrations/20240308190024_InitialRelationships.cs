using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PixHub.Migrations
{
    /// <inheritdoc />
    public partial class InitialRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentProviderAccountId",
                table: "PixKey",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentProviderId",
                table: "PaymentProviderAccount",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "PaymentProviderAccount",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PixKey_PaymentProviderAccountId",
                table: "PixKey",
                column: "PaymentProviderAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentProviderAccount_PaymentProviderId",
                table: "PaymentProviderAccount",
                column: "PaymentProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentProviderAccount_UserId",
                table: "PaymentProviderAccount",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentProviderAccount_PaymentProvider_PaymentProviderId",
                table: "PaymentProviderAccount",
                column: "PaymentProviderId",
                principalTable: "PaymentProvider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentProviderAccount_User_UserId",
                table: "PaymentProviderAccount",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PixKey_PaymentProviderAccount_PaymentProviderAccountId",
                table: "PixKey",
                column: "PaymentProviderAccountId",
                principalTable: "PaymentProviderAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentProviderAccount_PaymentProvider_PaymentProviderId",
                table: "PaymentProviderAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentProviderAccount_User_UserId",
                table: "PaymentProviderAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_PixKey_PaymentProviderAccount_PaymentProviderAccountId",
                table: "PixKey");

            migrationBuilder.DropIndex(
                name: "IX_PixKey_PaymentProviderAccountId",
                table: "PixKey");

            migrationBuilder.DropIndex(
                name: "IX_PaymentProviderAccount_PaymentProviderId",
                table: "PaymentProviderAccount");

            migrationBuilder.DropIndex(
                name: "IX_PaymentProviderAccount_UserId",
                table: "PaymentProviderAccount");

            migrationBuilder.DropColumn(
                name: "PaymentProviderAccountId",
                table: "PixKey");

            migrationBuilder.DropColumn(
                name: "PaymentProviderId",
                table: "PaymentProviderAccount");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PaymentProviderAccount");
        }
    }
}
