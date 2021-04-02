using Microsoft.EntityFrameworkCore;
using ServicesModel.Models;
using ServicesModel.Models.Account;
using ServicesModel.Models.Auth;
using ServicesModel.Models.Categories;
using ServicesModel.Models.Clients;
using ServicesModel.Models.Images;
using ServicesModel.Models.Services;
using ServicesModel.Models.Shedule;
using ServicesModel.Models.Staff;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;


namespace ServicesModel.Context
{
  public  class ServicesContext: DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Change_Password> change_Passwords { get; set; }
       public DbSet<Auth> Auths { get; set; }
        public DbSet<CategoryAccount> categoryAccounts { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<FotoService> fotoServices { get; set; }
        public DbSet<UserpicStaff> UserpicsStaff { get; set; }
        public DbSet<EmployeeOwner> EmployeeOwners { get; set; }
        public DbSet<Confirm> Confirms { get; set; }
        public DbSet<Userpic> Userpics { get; set; }
        public DbSet<PhotoServices> photoServices { get; set; }
        public DbSet<DayOfWork> dayOfWorks { get; set; }
        public DbSet<ConctereDay> conctereDays { get; set; }
        public DbSet<Shedule> Shedules { get; set; }
        public DbSet<StaffService> Services { get; set; }

        public ServicesContext(DbContextOptions<ServicesContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Auth>()
                .HasIndex(u => u.email)
                .IsUnique();
                  }
    }
}
