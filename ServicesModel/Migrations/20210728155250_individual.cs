using Microsoft.EntityFrameworkCore.Migrations;

namespace ServicesModel.Migrations
{
    public partial class individual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isIndividual",
                table: "Accounts",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isIndividual",
                table: "Accounts");
        }
    }
}
