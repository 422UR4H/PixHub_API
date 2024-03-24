using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PixHub.Migrations
{
    /// <inheritdoc />
    public partial class Add_Idx_User_CPF : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "Idx_User_CPF",
                table: "User",
                column: "CPF",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Idx_User_CPF",
                table: "User");
        }
    }
}
