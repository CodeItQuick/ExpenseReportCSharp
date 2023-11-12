using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseReportCSharp.Migrations
{
    /// <inheritdoc />
    public partial class CreateExpenseReportAggregateForeignKeyReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExpenseReportAggregateId",
                table: "Expenses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpenseReportAggregateId",
                table: "Expenses");
        }
    }
}
