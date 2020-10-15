using Microsoft.EntityFrameworkCore.Migrations;

namespace Test_AITS.Migrations
{
    public partial class DealerInfo_Table_Altered : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InboundCommission",
                table: "DealerInfos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrdinalCommission",
                table: "DealerInfos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OutboundCommission",
                table: "DealerInfos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SalesCommission",
                table: "DealerInfos",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InboundCommission",
                table: "DealerInfos");

            migrationBuilder.DropColumn(
                name: "OrdinalCommission",
                table: "DealerInfos");

            migrationBuilder.DropColumn(
                name: "OutboundCommission",
                table: "DealerInfos");

            migrationBuilder.DropColumn(
                name: "SalesCommission",
                table: "DealerInfos");
        }
    }
}
