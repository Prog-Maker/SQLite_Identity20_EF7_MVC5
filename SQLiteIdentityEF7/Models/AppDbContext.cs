using Identity.SQLite;
using Microsoft.Data.Entity;
using SQLiteIdentityEF7.Identity;
using System;
using System.Web;

namespace SQLiteIdentityEF7.Models
{
    public class IdentityEntities : DbContext
    {
        string spath = "";

        private static bool _created = false;
        public IdentityEntities()
        {
            if (!_created)
            {
                Database.EnsureCreatedAsync();
                _created = true;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            spath = AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "\\" + "database.sqlite";

            optionsBuilder.UseSqlite("Data Source=" + spath);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetUserLogins>()
                 .HasKey(k => new { k.LoginProvider, k.ProviderKey, k.UserId });

            modelBuilder.Entity<AspNetUserRoles>()
                 .HasKey(k => new { k.UserId, k.RoleId });

            modelBuilder.Entity<AspNetUserRoles>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.AspNetUserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<AspNetUserRoles>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.AspNetUserRoles)
                .HasForeignKey(ur => ur.RoleId);


            modelBuilder.Entity<C__MigrationHistory>().HasKey(k => k.MigrationId);
 

         /*   modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);*/

            
            base.OnModelCreating(modelBuilder);
        }

        public static IdentityEntities Create()
        {
            return new IdentityEntities();
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }

    }

    public class MyHttpApplication : HttpApplication
    {
        public static string GetAppDataPath()
        {
            var str = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/");
            return str;
        }
    }

    

    



}