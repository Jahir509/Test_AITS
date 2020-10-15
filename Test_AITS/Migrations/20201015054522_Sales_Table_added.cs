using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Test_AITS.Migrations
{
    public partial class Sales_Table_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealerCode = table.Column<string>(nullable: true),
                    SellAmount = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    SalesCommission = table.Column<int>(nullable: false),
                    InboundCommission = table.Column<int>(nullable: false),
                    OutboundCommission = table.Column<int>(nullable: false),
                    OrdinalCommission = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sales");

        }
    }
}
