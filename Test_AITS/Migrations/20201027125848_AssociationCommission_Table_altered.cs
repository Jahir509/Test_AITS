using Microsoft.EntityFrameworkCore.Migrations;

namespace Test_AITS.Migrations
{
    public partial class AssociationCommission_Table_altered : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssociationCommissions_Sales_SaleId",
                table: "AssociationCommissions");

            migrationBuilder.DropIndex(
                name: "IX_AssociationCommissions_SaleId",
                table: "AssociationCommissions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AssociationCommissions_SaleId",
                table: "AssociationCommissions",
                column: "SaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssociationCommissions_Sales_SaleId",
                table: "AssociationCommissions",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
