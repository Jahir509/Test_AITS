using Microsoft.EntityFrameworkCore.Migrations;

namespace Test_AITS.Migrations
{
    public partial class DealerInfo_Table_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DealerInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealerCode = table.Column<string>(nullable: true),
                    SellAmount = table.Column<int>(nullable: false),
                    DealerId = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_DealerInfos_DealerId",
                table: "DealerInfos",
                column: "DealerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DealerInfos");
        }
    }
}
