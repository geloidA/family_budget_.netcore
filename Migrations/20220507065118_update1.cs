using Microsoft.EntityFrameworkCore.Migrations;

namespace family_budget.Migrations
{
    public partial class update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_FamilyMembers_FamilyMemberId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Incomes_FamilyMembers_FamilyMemberId",
                table: "Incomes");

            migrationBuilder.DropIndex(
                name: "IX_Incomes_FamilyMemberId",
                table: "Incomes");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_FamilyMemberId",
                table: "Expenses");

            migrationBuilder.AlterColumn<int>(
                name: "FamilyMemberId",
                table: "Incomes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Cost",
                table: "Incomes",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "FamilyMemberId",
                table: "Expenses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Cost",
                table: "Expenses",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "FamilyMemberId",
                table: "Incomes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "Incomes",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "FamilyMemberId",
                table: "Expenses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "Expenses",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_FamilyMemberId",
                table: "Incomes",
                column: "FamilyMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_FamilyMemberId",
                table: "Expenses",
                column: "FamilyMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_FamilyMembers_FamilyMemberId",
                table: "Expenses",
                column: "FamilyMemberId",
                principalTable: "FamilyMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Incomes_FamilyMembers_FamilyMemberId",
                table: "Incomes",
                column: "FamilyMemberId",
                principalTable: "FamilyMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
