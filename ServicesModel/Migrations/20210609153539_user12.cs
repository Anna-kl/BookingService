using Microsoft.EntityFrameworkCore.Migrations;

namespace ServicesModel.Migrations
{
    public partial class user12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "id_user",
                table: "Uids",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Uids_id_user",
                table: "Uids",
                column: "id_user");

            migrationBuilder.AddForeignKey(
                name: "FK_Uids_Auths_id_user",
                table: "Uids",
                column: "id_user",
                principalTable: "Auths",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Uids_Auths_id_user",
                table: "Uids");

            migrationBuilder.DropIndex(
                name: "IX_Uids_id_user",
                table: "Uids");

            migrationBuilder.DropColumn(
                name: "id_user",
                table: "Uids");
        }
    }
}
