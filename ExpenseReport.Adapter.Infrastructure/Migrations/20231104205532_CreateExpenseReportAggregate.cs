using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseReportCSharp.Migrations
{
    /// <inheritdoc />
    public partial class CreateExpenseReportAggregate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExpensesReportAggregateId",
                table: "Expenses",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExpenseReportAggregates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseReportAggregates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ExpensesReportAggregateId",
                table: "Expenses",
                column: "ExpensesReportAggregateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_ExpenseReportAggregates_ExpensesReportAggregateId",
                table: "Expenses",
                column: "ExpensesReportAggregateId",
                principalTable: "ExpenseReportAggregates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_ExpenseReportAggregates_ExpensesReportAggregateId",
                table: "Expenses");

            migrationBuilder.DropTable(
                name: "ExpenseReportAggregates");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_ExpensesReportAggregateId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "ExpensesReportAggregateId",
                table: "Expenses");
        }
    }
}
