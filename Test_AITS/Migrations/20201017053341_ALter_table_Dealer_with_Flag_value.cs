using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Test_AITS.Migrations
{
    public partial class ALter_table_Dealer_with_Flag_value : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Date",
                table: "Sales",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "IsAMC",
                table: "Dealers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IsBMC",
                table: "Dealers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IsSIDC",
                table: "Dealers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAMC",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "IsBMC",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "IsSIDC",
                table: "Dealers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Sales",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
