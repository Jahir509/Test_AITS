using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Test_AITS.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Commissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(nullable: true),
                    FromAmount = table.Column<int>(nullable: false),
                    ToAmount = table.Column<int>(nullable: false),
                    Percentage = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DealerTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerTypes", x => x.Id);
                });

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
                    OrdinalCommission = table.Column<int>(nullable: false),
                    GBCommission = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalesCommissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Designation = table.Column<string>(nullable: true),
                    Percentage = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesCommissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Thanas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Thanas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dealers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    DealerTypeId = table.Column<int>(nullable: false),
                    ThanaId = table.Column<int>(nullable: false),
                    Designation = table.Column<string>(nullable: true),
                    IsSIDC = table.Column<int>(nullable: false),
                    IsAMC = table.Column<int>(nullable: false),
                    IsBMC = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dealers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dealers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dealers_DealerTypes_DealerTypeId",
                        column: x => x.DealerTypeId,
                        principalTable: "DealerTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dealers_Thanas_ThanaId",
                        column: x => x.ThanaId,
                        principalTable: "Thanas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DealerInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealerCode = table.Column<string>(nullable: true),
                    SellAmount = table.Column<int>(nullable: false),
                    DealerId = table.Column<int>(nullable: false),
                    Designation = table.Column<string>(nullable: true),
                    AssociateOf = table.Column<string>(nullable: true),
                    SalesCommission = table.Column<int>(nullable: false),
                    InboundCommission = table.Column<int>(nullable: false),
                    OutboundCommission = table.Column<int>(nullable: false),
                    OrdinalCommission = table.Column<int>(nullable: false),
                    GBCommission = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealerInfos_Dealers_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Commissions",
                columns: new[] { "Id", "FromAmount", "Percentage", "ToAmount", "Type" },
                values: new object[,]
                {
                    { 1, 1000, 6f, 10000, "SIDC" },
                    { 2, 10000, 7.5f, 30000, "SIDC" },
                    { 3, 30000, 9f, 60000, "SIDC" },
                    { 4, 60000, 11f, 999999, "SIDC" },
                    { 5, 5000, 5f, 50000, "AMC" },
                    { 6, 50000, 7f, 999999, "AMC" },
                    { 7, 5000, 5f, 999999, "BMC" }
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Walton" });

            migrationBuilder.InsertData(
                table: "DealerTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 2, "AMC" },
                    { 3, "BMC" },
                    { 1, "SIDC" }
                });

            migrationBuilder.InsertData(
                table: "SalesCommissions",
                columns: new[] { "Id", "Designation", "Percentage" },
                values: new object[,]
                {
                    { 10, "K", 26f },
                    { 1, "B", 12.5f },
                    { 2, "C", 15f },
                    { 3, "D", 17.5f },
                    { 4, "E", 20f },
                    { 5, "F", 21f },
                    { 6, "G", 22f },
                    { 7, "H", 23f },
                    { 8, "I", 24f },
                    { 9, "J", 25f }
                });

            migrationBuilder.InsertData(
                table: "Thanas",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Thana-A" });

            migrationBuilder.InsertData(
                table: "Dealers",
                columns: new[] { "Id", "CompanyId", "DealerTypeId", "Designation", "IsAMC", "IsBMC", "IsSIDC", "Name", "ThanaId" },
                values: new object[] { 1, 1, 1, null, 0, 0, 1, "Company", 1 });

            migrationBuilder.InsertData(
                table: "DealerInfos",
                columns: new[] { "Id", "AssociateOf", "DealerCode", "DealerId", "Designation", "GBCommission", "InboundCommission", "OrdinalCommission", "OutboundCommission", "SalesCommission", "SellAmount" },
                values: new object[] { 1, "Company", "E-0001", 1, "Company", 0, 0, 0, 0, 0, 0 });

            migrationBuilder.CreateIndex(
                name: "IX_DealerInfos_DealerId",
                table: "DealerInfos",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_Dealers_CompanyId",
                table: "Dealers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Dealers_DealerTypeId",
                table: "Dealers",
                column: "DealerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Dealers_ThanaId",
                table: "Dealers",
                column: "ThanaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Commissions");

            migrationBuilder.DropTable(
                name: "DealerInfos");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "SalesCommissions");

            migrationBuilder.DropTable(
                name: "Dealers");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "DealerTypes");

            migrationBuilder.DropTable(
                name: "Thanas");
        }
    }
}
