using Microsoft.EntityFrameworkCore.Migrations;

namespace ServicesModel.Migrations
{
    public partial class specialist1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "specialization",
                table: "Accounts");

            migrationBuilder.AddColumn<string>(
                name: "specialization",
                table: "EmployeeOwners",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "specialization",
                table: "EmployeeOwners");

            migrationBuilder.AddColumn<string>(
                name: "specialization",
                table: "Accounts",
                type: "text",
                nullable: true);
        }
    }
}
