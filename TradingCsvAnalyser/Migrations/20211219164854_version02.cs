using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingCsvAnalyser.Migrations
{
    public partial class version02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "PriceEntries");

            migrationBuilder.DropColumn(
                name: "Day",
                table: "PriceEntries");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "PriceEntries",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "Day",
                table: "PriceEntries",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
