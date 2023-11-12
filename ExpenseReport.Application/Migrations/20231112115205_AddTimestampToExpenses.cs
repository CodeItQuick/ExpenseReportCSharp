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
                name: "FK_Expenses_ExpenseReportAggregates_ExpensesReportAggregateId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_ExpensesReportAggregateId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "ExpensesReportAggregateId",
                table: "Expenses");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpenseReportDate",
                table: "ExpenseReportAggregates",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ExpenseReportAggregateId",
                table: "Expenses",
                column: "ExpenseReportAggregateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_ExpenseReportAggregates_ExpenseReportAggregateId",
                table: "Expenses",
                column: "ExpenseReportAggregateId",
                principalTable: "ExpenseReportAggregates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_ExpenseReportAggregates_ExpenseReportAggregateId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_ExpenseReportAggregateId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "ExpenseReportDate",
                table: "ExpenseReportAggregates");

            migrationBuilder.AddColumn<int>(
                name: "ExpensesReportAggregateId",
                table: "Expenses",
                type: "INTEGER",
                nullable: true);

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
    }
}
