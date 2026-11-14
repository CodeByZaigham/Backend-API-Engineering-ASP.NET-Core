using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shopforge_backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatedcart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "Cart");

            migrationBuilder.AddColumn<bool>(
                name: "IsOffer",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Rating",
                table: "Products",
                type: "decimal(3,1)",
                precision: 3,
                scale: 1,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Cart_UserId_ProductId",
                table: "Cart",
                columns: new[] { "UserId", "ProductId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Users_UserId",
                table: "Cart",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Users_UserId",
                table: "Cart");

            migrationBuilder.DropIndex(
                name: "IX_Cart_UserId_ProductId",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "IsOffer",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Products");

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "Cart",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
