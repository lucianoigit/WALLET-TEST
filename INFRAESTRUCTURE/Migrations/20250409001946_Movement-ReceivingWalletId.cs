using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INFRAESTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class MovementReceivingWalletId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "receiving_wallet_id",
                table: "Movements",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "receiving_wallet_id",
                table: "Movements");
        }
    }
}
