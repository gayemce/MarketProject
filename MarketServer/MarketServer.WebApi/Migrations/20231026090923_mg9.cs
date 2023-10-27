using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketServer.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class mg9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OrderNumber",
                table: "OrderStatues",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatues_Status_OrderNumber",
                table: "OrderStatues",
                columns: new[] { "Status", "OrderNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderStatues_Status_OrderNumber",
                table: "OrderStatues");

            migrationBuilder.AlterColumn<string>(
                name: "OrderNumber",
                table: "OrderStatues",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
