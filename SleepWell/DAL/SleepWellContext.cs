using Microsoft.AspNet.Identity.EntityFramework;
using SleepWell.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SleepWell.DAL
{
    public class SleepWellContext : IdentityDbContext<User>
    {
        public SleepWellContext() : base("SleepWell") { }

        static SleepWellContext()
        {
            //Database.SetInitializer<SystemContext>(new SystemInitializer());
        }

        public static SleepWellContext Create()
        {
            return new SleepWellContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Reservation>().HasRequired(o => o.User);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
        }

        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Reclamation> Reclamations { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Setting> Settings { get; set; }
    }
}