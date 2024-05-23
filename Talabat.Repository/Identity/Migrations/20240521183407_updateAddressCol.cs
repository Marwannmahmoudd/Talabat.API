using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talabat.Repository.identity.migrations
{
    public partial class updateAddressCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Lname",
                table: "Addresses",
                newName: "Lastname");

            migrationBuilder.RenameColumn(
                name: "Fname",
                table: "Addresses",
                newName: "Firstname");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Lastname",
                table: "Addresses",
                newName: "Lname");

            migrationBuilder.RenameColumn(
                name: "Firstname",
                table: "Addresses",
                newName: "Fname");
        }
    }
}
