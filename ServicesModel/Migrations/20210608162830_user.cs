using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ServicesModel.Migrations
{
    public partial class user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Auths",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Auths",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "site",
                table: "Accounts",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Uids",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uid = table.Column<string>(nullable: true),
                    updateDttm = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uids", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Uids");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Auths");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Auths");

            migrationBuilder.DropColumn(
                name: "site",
                table: "Accounts");
        }
    }
}
