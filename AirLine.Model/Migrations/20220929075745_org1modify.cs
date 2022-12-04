using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirLine.Model.Migrations
{
    public partial class org1modify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Tbl_EmpOrganization",
                newName: "OrganizationName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrganizationName",
                table: "Tbl_EmpOrganization",
                newName: "Name");
        }
    }
}
