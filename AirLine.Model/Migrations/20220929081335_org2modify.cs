using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirLine.Model.Migrations
{
    public partial class org2modify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_EmpOrganization_Tbl_Employee_FEmployeeId",
                table: "Tbl_EmpOrganization");

            migrationBuilder.DropIndex(
                name: "IX_Tbl_EmpOrganization_FEmployeeId",
                table: "Tbl_EmpOrganization");

            migrationBuilder.AddColumn<int>(
                name: "FEmployeeId",
                table: "Tbl_Employee",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_Employee_FEmployeeId",
                table: "Tbl_Employee",
                column: "FEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_Employee_Tbl_EmpOrganization_FEmployeeId",
                table: "Tbl_Employee",
                column: "FEmployeeId",
                principalTable: "Tbl_EmpOrganization",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_Employee_Tbl_EmpOrganization_FEmployeeId",
                table: "Tbl_Employee");

            migrationBuilder.DropIndex(
                name: "IX_Tbl_Employee_FEmployeeId",
                table: "Tbl_Employee");

            migrationBuilder.DropColumn(
                name: "FEmployeeId",
                table: "Tbl_Employee");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_EmpOrganization_FEmployeeId",
                table: "Tbl_EmpOrganization",
                column: "FEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_EmpOrganization_Tbl_Employee_FEmployeeId",
                table: "Tbl_EmpOrganization",
                column: "FEmployeeId",
                principalTable: "Tbl_Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
