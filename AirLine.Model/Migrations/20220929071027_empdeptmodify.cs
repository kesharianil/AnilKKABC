using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirLine.Model.Migrations
{
    public partial class empdeptmodify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_EmpOrganization_Tbl_Employee_EmployeeId",
                table: "Tbl_EmpOrganization");

            migrationBuilder.DropIndex(
                name: "IX_Tbl_EmpOrganization_EmployeeId",
                table: "Tbl_EmpOrganization");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Tbl_EmpOrganization");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "Tbl_EmpOrganization",
                newName: "FEmployeeId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_EmpOrganization_Tbl_Employee_FEmployeeId",
                table: "Tbl_EmpOrganization");

            migrationBuilder.DropIndex(
                name: "IX_Tbl_EmpOrganization_FEmployeeId",
                table: "Tbl_EmpOrganization");

            migrationBuilder.RenameColumn(
                name: "FEmployeeId",
                table: "Tbl_EmpOrganization",
                newName: "OrganizationId");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Tbl_EmpOrganization",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_EmpOrganization_EmployeeId",
                table: "Tbl_EmpOrganization",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_EmpOrganization_Tbl_Employee_EmployeeId",
                table: "Tbl_EmpOrganization",
                column: "EmployeeId",
                principalTable: "Tbl_Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
