using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirLine.Model.Migrations
{
    public partial class orglistmodify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_Employee_Tbl_EmpOrganization_FEmployeeId",
                table: "Tbl_Employee");

            migrationBuilder.RenameColumn(
                name: "FEmployeeId",
                table: "Tbl_Employee",
                newName: "EmployeeId1");

            migrationBuilder.RenameIndex(
                name: "IX_Tbl_Employee_FEmployeeId",
                table: "Tbl_Employee",
                newName: "IX_Tbl_Employee_EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_EmpOrganization_FEmployeeId",
                table: "Tbl_EmpOrganization",
                column: "FEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_Employee_Tbl_Employee_EmployeeId1",
                table: "Tbl_Employee",
                column: "EmployeeId1",
                principalTable: "Tbl_Employee",
                principalColumn: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_EmpOrganization_Tbl_Employee_FEmployeeId",
                table: "Tbl_EmpOrganization",
                column: "FEmployeeId",
                principalTable: "Tbl_Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_Employee_Tbl_Employee_EmployeeId1",
                table: "Tbl_Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_EmpOrganization_Tbl_Employee_FEmployeeId",
                table: "Tbl_EmpOrganization");

            migrationBuilder.DropIndex(
                name: "IX_Tbl_EmpOrganization_FEmployeeId",
                table: "Tbl_EmpOrganization");

            migrationBuilder.RenameColumn(
                name: "EmployeeId1",
                table: "Tbl_Employee",
                newName: "FEmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Tbl_Employee_EmployeeId1",
                table: "Tbl_Employee",
                newName: "IX_Tbl_Employee_FEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_Employee_Tbl_EmpOrganization_FEmployeeId",
                table: "Tbl_Employee",
                column: "FEmployeeId",
                principalTable: "Tbl_EmpOrganization",
                principalColumn: "Id");
        }
    }
}
