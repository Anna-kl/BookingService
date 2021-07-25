using Microsoft.EntityFrameworkCore.Migrations;

namespace ServicesModel.Migrations
{
    public partial class specialist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "specialization",
                table: "Accounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "specialization",
                table: "Accounts");
        }
    }
}
