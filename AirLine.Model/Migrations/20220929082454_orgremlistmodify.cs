using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirLine.Model.Migrations
{
    public partial class orgremlistmodify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_Employee_Tbl_Employee_EmployeeId1",
                table: "Tbl_Employee");

            migrationBuilder.DropIndex(
                name: "IX_Tbl_Employee_EmployeeId1",
                table: "Tbl_Employee");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                table: "Tbl_Employee");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId1",
                table: "Tbl_Employee",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_Employee_EmployeeId1",
                table: "Tbl_Employee",
                column: "EmployeeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_Employee_Tbl_Employee_EmployeeId1",
                table: "Tbl_Employee",
                column: "EmployeeId1",
                principalTable: "Tbl_Employee",
                principalColumn: "EmployeeId");
        }
    }
}
