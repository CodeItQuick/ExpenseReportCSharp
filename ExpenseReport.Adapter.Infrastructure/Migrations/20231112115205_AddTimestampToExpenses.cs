using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseReportCSharp.Migrations
{
    /// <inheritdoc />
    public partial class AddTimestampToExpenses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_ExpenseReports_ExpensesReportId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_ExpensesReportId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "ExpensesReportId",
                table: "Expenses");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpenseReportDate",
                table: "ExpenseReports",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ExpenseReportId",
                table: "Expenses",
                column: "ExpenseReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_ExpenseReports_ExpenseReportId",
                table: "Expenses",
                column: "ExpenseReportId",
                principalTable: "ExpenseReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_ExpenseReports_ExpenseReportId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_ExpenseReportId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "ExpenseReportDate",
                table: "ExpenseReports");

            migrationBuilder.AddColumn<int>(
                name: "ExpensesReportId",
                table: "Expenses",
                type: "INTEGER",
                nullable: true);

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
    }
}
