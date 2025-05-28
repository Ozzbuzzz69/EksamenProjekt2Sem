using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EksamenProjekt2Sem.Migrations
{
    /// <inheritdoc />
    public partial class hejhej : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CampaignOffers_CampaignOfferId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Food_FoodId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "FoodId",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CampaignOfferId",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CampaignOffers_CampaignOfferId",
                table: "Orders",
                column: "CampaignOfferId",
                principalTable: "CampaignOffers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Food_FoodId",
                table: "Orders",
                column: "FoodId",
                principalTable: "Food",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CampaignOffers_CampaignOfferId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Food_FoodId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "FoodId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CampaignOfferId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CampaignOffers_CampaignOfferId",
                table: "Orders",
                column: "CampaignOfferId",
                principalTable: "CampaignOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Food_FoodId",
                table: "Orders",
                column: "FoodId",
                principalTable: "Food",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
