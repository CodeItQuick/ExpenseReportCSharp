using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseReportCSharp.Migrations
{
    /// <inheritdoc />
    public partial class CreateExpenseReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExpensesReportId",
                table: "Expenses",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExpenseReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseReport", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ExpensesReportId",
                table: "Expenses",
                column: "ExpensesReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_ExpenseReports_ExpensesReportId",
                table: "Expenses",
                column: "ExpensesReportId",
                principalTable: "ExpenseReports",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_ExpenseReports_ExpensesReportId",
                table: "Expenses");

            migrationBuilder.DropTable(
                name: "ExpenseReports");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_ExpensesReportId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "ExpensesReportId",
                table: "Expenses");
        }
    }
}
