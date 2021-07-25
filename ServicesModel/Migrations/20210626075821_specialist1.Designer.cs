﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ServicesModel.Context;

namespace ServicesModel.Migrations
{
    [DbContext(typeof(ServicesContext))]
    [Migration("20210626075821_specialist1")]
    partial class specialist1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("ServicesModel.Models.Account.Account", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("address")
                        .HasColumnType("text");

                    b.Property<string>("email")
                        .HasColumnType("text");

                    b.Property<int>("id_user")
                        .HasColumnType("integer");

                    b.Property<string>("name")
                        .HasColumnType("text");

                    b.Property<string>("phone")
                        .HasColumnType("text");

                    b.Property<string>("site")
                        .HasColumnType("text");

                    b.Property<DateTime>("update")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("id");

                    b.HasIndex("id_user");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("ServicesModel.Models.Account.CategoryAccount", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("id_account")
                        .HasColumnType("integer");

                    b.Property<int>("level0")
                        .HasColumnType("integer");

                    b.Property<int>("level1")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.HasIndex("id_account");

                    b.ToTable("categoryAccounts");
                });

            modelBuilder.Entity("ServicesModel.Models.Auth.Auth", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.Property<DateTime>("data_add")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("email")
                        .HasColumnType("text");

                    b.Property<bool>("is_active")
                        .HasColumnType("boolean");

                    b.Property<bool>("is_confirm")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("last_visit")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("email")
                        .IsUnique();

                    b.ToTable("Auths");
                });

            modelBuilder.Entity("ServicesModel.Models.Auth.Change_Password", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("dttm")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("password")
                        .HasColumnType("text");

                    b.Property<bool>("state")
                        .HasColumnType("boolean");

                    b.Property<int>("user_id")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.ToTable("change_Passwords");
                });

            modelBuilder.Entity("ServicesModel.Models.Auth.Confirm", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("authid")
                        .HasColumnType("integer");

                    b.Property<string>("code")
                        .HasColumnType("text");

                    b.Property<DateTime>("send")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("user_id")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.HasIndex("authid");

                    b.ToTable("Confirms");
                });

            modelBuilder.Entity("ServicesModel.Models.Auth.Token", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("access")
                        .HasColumnType("text");

                    b.Property<DateTime>("access_expire")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("access_generate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("refresh")
                        .HasColumnType("text");

                    b.Property<DateTime>("refresh_expire")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("refresh_generate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("user_id")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.HasIndex("user_id");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("ServicesModel.Models.Auth.UID", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("id_user")
                        .HasColumnType("integer");

                    b.Property<string>("uid")
                        .HasColumnType("text");

                    b.Property<DateTime>("updateDttm")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("id");

                    b.HasIndex("id_user");

                    b.ToTable("Uids");
                });

            modelBuilder.Entity("ServicesModel.Models.Categories.Category", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("level")
                        .HasColumnType("integer");

                    b.Property<string>("name")
                        .HasColumnType("text");

                    b.Property<int>("parent")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ServicesModel.Models.Clients.Client", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("desc")
                        .HasColumnType("text");

                    b.Property<string>("email")
                        .HasColumnType("text");

                    b.Property<int>("id_user")
                        .HasColumnType("integer");

                    b.Property<string>("name")
                        .HasColumnType("text");

                    b.Property<string>("phone")
                        .HasColumnType("text");

                    b.Property<string>("status")
                        .HasColumnType("text");

                    b.Property<DateTime>("update_date")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("id");

                    b.HasIndex("id_user");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("ServicesModel.Models.Geo.Coordinate", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("account_id")
                        .HasColumnType("integer");

                    b.Property<double>("lat")
                        .HasColumnType("double precision");

                    b.Property<double>("lon")
                        .HasColumnType("double precision");

                    b.HasKey("id");

                    b.HasIndex("account_id");

                    b.ToTable("Coordinates");
                });

            modelBuilder.Entity("ServicesModel.Models.Images.PhotoServices", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("dttmadd")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("id_service")
                        .HasColumnType("integer");

                    b.Property<string>("name")
                        .HasColumnType("text");

                    b.Property<string>("path")
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("id_service");

                    b.ToTable("photoServices");
                });

            modelBuilder.Entity("ServicesModel.Models.Images.Userpic", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("account_id")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("dttmadd")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("name")
                        .HasColumnType("text");

                    b.Property<string>("path")
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("Userpics");
                });

            modelBuilder.Entity("ServicesModel.Models.Images.UserpicStaff", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("account_id")
                        .HasColumnType("integer");

                    b.Property<DateTime>("dttmadd")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("name")
                        .HasColumnType("text");

                    b.Property<string>("path")
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("account_id");

                    b.ToTable("UserpicsStaff");
                });

            modelBuilder.Entity("ServicesModel.Models.Services.FotoService", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("id_services")
                        .HasColumnType("integer");

                    b.Property<string>("name")
                        .HasColumnType("text");

                    b.Property<string>("path")
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("fotoServices");
                });

            modelBuilder.Entity("ServicesModel.Models.Shedule.ConctereDay", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("client_id")
                        .HasColumnType("integer");

                    b.Property<string>("comment")
                        .HasColumnType("text");

                    b.Property<int>("daysof")
                        .HasColumnType("integer");

                    b.Property<DateTime>("dttm_end")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("dttm_start")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("is_complete")
                        .HasColumnType("boolean");

                    b.Property<bool>("iscanceled")
                        .HasColumnType("boolean");

                    b.Property<float>("price")
                        .HasColumnType("real");

                    b.Property<string>("services_comment")
                        .HasColumnType("text");

                    b.Property<int>("services_id")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.HasIndex("daysof");

                    b.ToTable("conctereDays");
                });

            modelBuilder.Entity("ServicesModel.Models.Shedule.DayOfWork", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("accountId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("dttmEnd")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("dttmStart")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("id");

                    b.HasIndex("accountId");

                    b.ToTable("dayOfWorks");
                });

            modelBuilder.Entity("ServicesModel.Models.Staff.EmployeeOwner", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("accepted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("birthday")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("date_add")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("email")
                        .HasColumnType("text");

                    b.Property<string>("firstname")
                        .HasColumnType("text");

                    b.Property<int>("id_owner")
                        .HasColumnType("integer");

                    b.Property<int>("id_user")
                        .HasColumnType("integer");

                    b.Property<string>("lastname")
                        .HasColumnType("text");

                    b.Property<string>("link")
                        .HasColumnType("text");

                    b.Property<string>("middlename")
                        .HasColumnType("text");

                    b.Property<string>("phone")
                        .HasColumnType("text");

                    b.Property<string>("position")
                        .HasColumnType("text");

                    b.Property<string>("specialization")
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("id_user");

                    b.ToTable("EmployeeOwners");
                });

            modelBuilder.Entity("ServicesModel.Models.Staff.Shedule", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("complete")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("dttm_end")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("dttm_start")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("service_id")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.HasIndex("service_id");

                    b.ToTable("Shedules");
                });

            modelBuilder.Entity("ServicesModel.Models.StaffService", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("account_id")
                        .HasColumnType("integer");

                    b.Property<int>("category")
                        .HasColumnType("integer");

                    b.Property<string>("descride")
                        .HasColumnType("text");

                    b.Property<int>("minutes")
                        .HasColumnType("integer");

                    b.Property<string>("name")
                        .HasColumnType("text");

                    b.Property<float>("price")
                        .HasColumnType("real");

                    b.HasKey("id");

                    b.HasIndex("account_id");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("ServicesModel.Models.Account.Account", b =>
                {
                    b.HasOne("ServicesModel.Models.Auth.Auth", "Auth")
                        .WithMany()
                        .HasForeignKey("id_user")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ServicesModel.Models.Account.CategoryAccount", b =>
                {
                    b.HasOne("ServicesModel.Models.Account.Account", "Account")
                        .WithMany()
                        .HasForeignKey("id_account")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ServicesModel.Models.Auth.Confirm", b =>
                {
                    b.HasOne("ServicesModel.Models.Auth.Auth", "auth")
                        .WithMany()
                        .HasForeignKey("authid");
                });

            modelBuilder.Entity("ServicesModel.Models.Auth.Token", b =>
                {
                    b.HasOne("ServicesModel.Models.Auth.Auth", "Auth")
                        .WithMany()
                        .HasForeignKey("user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ServicesModel.Models.Auth.UID", b =>
                {
                    b.HasOne("ServicesModel.Models.Auth.Auth", "Auth")
                        .WithMany()
                        .HasForeignKey("id_user")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ServicesModel.Models.Clients.Client", b =>
                {
                    b.HasOne("ServicesModel.Models.Auth.Auth", "Auth")
                        .WithMany()
                        .HasForeignKey("id_user")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ServicesModel.Models.Geo.Coordinate", b =>
                {
                    b.HasOne("ServicesModel.Models.Account.Account", "Account")
                        .WithMany()
                        .HasForeignKey("account_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ServicesModel.Models.Images.PhotoServices", b =>
                {
                    b.HasOne("ServicesModel.Models.StaffService", "StaffService")
                        .WithMany()
                        .HasForeignKey("id_service")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ServicesModel.Models.Images.UserpicStaff", b =>
                {
                    b.HasOne("ServicesModel.Models.Staff.EmployeeOwner", "EmployeeOwner")
                        .WithMany()
                        .HasForeignKey("account_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ServicesModel.Models.Shedule.ConctereDay", b =>
                {
                    b.HasOne("ServicesModel.Models.Shedule.DayOfWork", "DayOfWork")
                        .WithMany()
                        .HasForeignKey("daysof")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ServicesModel.Models.Shedule.DayOfWork", b =>
                {
                    b.HasOne("ServicesModel.Models.Staff.EmployeeOwner", "EmployeeOwner")
                        .WithMany()
                        .HasForeignKey("accountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ServicesModel.Models.Staff.EmployeeOwner", b =>
                {
                    b.HasOne("ServicesModel.Models.Auth.Auth", "Auth")
                        .WithMany()
                        .HasForeignKey("id_user")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ServicesModel.Models.Staff.Shedule", b =>
                {
                    b.HasOne("ServicesModel.Models.StaffService", "Service")
                        .WithMany()
                        .HasForeignKey("service_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ServicesModel.Models.StaffService", b =>
                {
                    b.HasOne("ServicesModel.Models.Staff.EmployeeOwner", "EmployeeOwner")
                        .WithMany()
                        .HasForeignKey("account_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}