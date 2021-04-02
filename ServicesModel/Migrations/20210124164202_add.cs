using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ServicesModel.Migrations
{
    public partial class add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Auths",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    password = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    data_add = table.Column<DateTime>(nullable: false),
                    is_active = table.Column<bool>(nullable: false),
                    role = table.Column<string>(nullable: false),
                    last_visit = table.Column<DateTime>(nullable: false),
                    is_confirm = table.Column<bool>(nullable: false),
                    EmailConfirmed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auths", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    level = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    parent = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "change_Passwords",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(nullable: false),
                    dttm = table.Column<DateTime>(nullable: false),
                    state = table.Column<bool>(nullable: false),
                    password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_change_Passwords", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Userpics",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(nullable: true),
                    dttmadd = table.Column<DateTime>(nullable: false),
                    path = table.Column<string>(nullable: true),
                    account_id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Userpics", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(nullable: true),
                    address = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    phone = table.Column<string>(nullable: true),
                    id_user = table.Column<int>(nullable: false),
                    update = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.id);
                    table.ForeignKey(
                        name: "FK_Accounts_Auths_id_user",
                        column: x => x.id_user,
                        principalTable: "Auths",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(nullable: true),
                    phone = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    update_date = table.Column<DateTime>(nullable: false),
                    desc = table.Column<string>(nullable: true),
                    id_user = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.id);
                    table.ForeignKey(
                        name: "FK_Clients_Auths_id_user",
                        column: x => x.id_user,
                        principalTable: "Auths",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Confirms",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(nullable: false),
                    code = table.Column<string>(nullable: true),
                    send = table.Column<DateTime>(nullable: false),
                    authid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Confirms", x => x.id);
                    table.ForeignKey(
                        name: "FK_Confirms_Auths_authid",
                        column: x => x.authid,
                        principalTable: "Auths",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeOwners",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    firstname = table.Column<string>(nullable: true),
                    lastname = table.Column<string>(nullable: true),
                    middlename = table.Column<string>(nullable: true),
                    phone = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    link = table.Column<string>(nullable: true),
                    date_add = table.Column<DateTime>(nullable: false),
                    birthday = table.Column<DateTime>(nullable: false),
                    position = table.Column<string>(nullable: true),
                    id_owner = table.Column<int>(nullable: false),
                    accepted = table.Column<bool>(nullable: false),
                    id_user = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeOwners", x => x.id);
                    table.ForeignKey(
                        name: "FK_EmployeeOwners_Auths_id_user",
                        column: x => x.id_user,
                        principalTable: "Auths",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(nullable: false),
                    access = table.Column<string>(nullable: true),
                    refresh = table.Column<string>(nullable: true),
                    access_generate = table.Column<DateTime>(nullable: false),
                    refresh_generate = table.Column<DateTime>(nullable: false),
                    access_expire = table.Column<DateTime>(nullable: false),
                    refresh_expire = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.id);
                    table.ForeignKey(
                        name: "FK_Tokens_Auths_user_id",
                        column: x => x.user_id,
                        principalTable: "Auths",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "categoryAccounts",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    level0 = table.Column<int>(nullable: false),
                    level1 = table.Column<int>(nullable: false),
                    id_account = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categoryAccounts", x => x.id);
                    table.ForeignKey(
                        name: "FK_categoryAccounts_Accounts_id_account",
                        column: x => x.id_account,
                        principalTable: "Accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dayOfWorks",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    dttmStart = table.Column<DateTime>(nullable: false),
                    dttmEnd = table.Column<DateTime>(nullable: false),
                    accountId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dayOfWorks", x => x.id);
                    table.ForeignKey(
                        name: "FK_dayOfWorks_EmployeeOwners_accountId",
                        column: x => x.accountId,
                        principalTable: "EmployeeOwners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    price = table.Column<float>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    descride = table.Column<string>(nullable: true),
                    minutes = table.Column<int>(nullable: false),
                    account_id = table.Column<int>(nullable: false),
                    category = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.id);
                    table.ForeignKey(
                        name: "FK_Services_EmployeeOwners_account_id",
                        column: x => x.account_id,
                        principalTable: "EmployeeOwners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserpicsStaff",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(nullable: true),
                    dttmadd = table.Column<DateTime>(nullable: false),
                    path = table.Column<string>(nullable: true),
                    account_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserpicsStaff", x => x.id);
                    table.ForeignKey(
                        name: "FK_UserpicsStaff_EmployeeOwners_account_id",
                        column: x => x.account_id,
                        principalTable: "EmployeeOwners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "conctereDays",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    account_id = table.Column<int>(nullable: false),
                    services_id = table.Column<int>(nullable: false),
                    dttm_start = table.Column<DateTime>(nullable: false),
                    dttm_end = table.Column<DateTime>(nullable: false),
                    price = table.Column<float>(nullable: false),
                    comment = table.Column<string>(nullable: true),
                    daysof = table.Column<int>(nullable: false),
                    client_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conctereDays", x => x.id);
                    table.ForeignKey(
                        name: "FK_conctereDays_dayOfWorks_daysof",
                        column: x => x.daysof,
                        principalTable: "dayOfWorks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shedules",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    dttm_start = table.Column<DateTime>(nullable: false),
                    dttm_end = table.Column<DateTime>(nullable: false),
                    service_id = table.Column<int>(nullable: false),
                    complete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shedules", x => x.id);
                    table.ForeignKey(
                        name: "FK_Shedules_Services_service_id",
                        column: x => x.service_id,
                        principalTable: "Services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_id_user",
                table: "Accounts",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_Auths_email",
                table: "Auths",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_categoryAccounts_id_account",
                table: "categoryAccounts",
                column: "id_account");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_id_user",
                table: "Clients",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_conctereDays_daysof",
                table: "conctereDays",
                column: "daysof");

            migrationBuilder.CreateIndex(
                name: "IX_Confirms_authid",
                table: "Confirms",
                column: "authid");

            migrationBuilder.CreateIndex(
                name: "IX_dayOfWorks_accountId",
                table: "dayOfWorks",
                column: "accountId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeOwners_id_user",
                table: "EmployeeOwners",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_Services_account_id",
                table: "Services",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_Shedules_service_id",
                table: "Shedules",
                column: "service_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_user_id",
                table: "Tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserpicsStaff_account_id",
                table: "UserpicsStaff",
                column: "account_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "categoryAccounts");

            migrationBuilder.DropTable(
                name: "change_Passwords");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "conctereDays");

            migrationBuilder.DropTable(
                name: "Confirms");

            migrationBuilder.DropTable(
                name: "Shedules");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "Userpics");

            migrationBuilder.DropTable(
                name: "UserpicsStaff");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "dayOfWorks");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "EmployeeOwners");

            migrationBuilder.DropTable(
                name: "Auths");
        }
    }
}
