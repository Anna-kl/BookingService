using Microsoft.EntityFrameworkCore.Migrations;

namespace ServicesModel.Migrations
{
    public partial class specialist2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "specialization",
                table: "EmployeeOwners");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "specialization",
                table: "EmployeeOwners",
                type: "text",
                nullable: true);
        }
    }
}
