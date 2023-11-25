using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseReportCSharp.Migrations
{
    /// <inheritdoc />
    public partial class CreateIsApproved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "ExpenseReport",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "ExpenseReport");
        }
    }
}
