using Microsoft.EntityFrameworkCore.Migrations;

namespace ServicesModel.Migrations
{
    public partial class test34 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_complete",
                table: "conctereDays",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "services_comment",
                table: "conctereDays",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_complete",
                table: "conctereDays");

            migrationBuilder.DropColumn(
                name: "services_comment",
                table: "conctereDays");
        }
    }
}
