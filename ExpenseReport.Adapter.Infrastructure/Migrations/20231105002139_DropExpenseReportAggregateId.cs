using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseReportCSharp.Migrations
{
    /// <inheritdoc />
    public partial class DropExpenseReportAggregateId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpenseReportAggregateId",
                table: "Expenses");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
