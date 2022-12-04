using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirLine.Model.Migrations
{
    public partial class createproc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = "Create Procedure SP_GetUnApprovedList\r\nas\r\nBegin\r\nselect AspNetUsers.Id,AspNetUsers.UserName,AspNetUsers.IsApprove,AspNetRoles.Name \r\nfrom AspNetUsers \r\ninner join AspNetUserRoles on AspNetUsers.Id=AspNetUserRoles.UserId \r\ninner join AspNetRoles on AspNetRoles.Id=AspNetUserRoles.RoleId \r\nwhere AspNetRoles.Name='Operator' \r\nEnd";
            migrationBuilder.Sql(procedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string procedure = "drop Procedure SP_GetUnApprovedList";
            migrationBuilder.Sql(procedure);
        }
    }
}
