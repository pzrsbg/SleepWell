using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SleepWell.Migrations;
using SleepWell.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace SleepWell.DAL
{
    public class SleepWellInitializer : MigrateDatabaseToLatestVersion<SleepWellContext, Configuration>
    {
        public static void SeedCustomData(SleepWellContext context)
        {
            var settings = new List<Setting>
            {
                new Setting(){ SettingId = 1, Name = "NazwaPensjonatu", Value = "Sleep Well" }
            };

            var rooms = new List<Room>
            {
                new Room(){ RoomId = 1, RoomName = "Apartament Złoty", MaxPeople = 2, UnitCost = 80, RoomStandard = RoomStandard.BedAndBreakfast, PhotoUrl = "ap01.jpg" },
                new Room(){ RoomId = 2, RoomName = "Apartament Platynowy", MaxPeople = 2, UnitCost = 140, RoomStandard = RoomStandard.Exclusive, PhotoUrl = "ap02.jpg" },
                new Room(){ RoomId = 3, RoomName = "Apartament Srebrny", MaxPeople = 3, UnitCost = 60, RoomStandard = RoomStandard.Economic, PhotoUrl = "ap03.jpg" }
            };

            settings.ForEach(s => context.Settings.AddOrUpdate(s));
            rooms.ForEach(r => context.Rooms.AddOrUpdate(r));
            context.SaveChanges();
        }

        public static void InitializeIdentityForEF(SleepWellContext context)
        {
            var userManager = new UserManager<User>(new UserStore<User>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            //var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            //var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();


            var user_a = userManager.FindByName("admin@sleepwell.pl");
            if (user_a == null)
            {
                user_a = new User { UserName = "admin@sleepwell.pl", Email = "admin@sleepwell.pl" };
                var result = userManager.Create(user_a, "P@ssword");
                result = userManager.SetLockoutEnabled(user_a.Id, false);
            }
            var user_m = userManager.FindByName("manager@sleepwell.pl");
            if (user_m == null)
            {
                user_m = new User { UserName = "manager@sleepwell.pl", Email = "manager@sleepwell.pl" };
                var result = userManager.Create(user_m, "P@ssword");
                result = userManager.SetLockoutEnabled(user_m.Id, false);
            }
            var user_r = userManager.FindByName("receptionist@sleepwell.pl");
            if (user_r == null)
            {
                user_r = new User { UserName = "receptionist@sleepwell.pl", Email = "receptionist@sleepwell.pl" };
                var result = userManager.Create(user_r, "P@ssword");
                result = userManager.SetLockoutEnabled(user_r.Id, false);
            }

            //tworzenie roli użytkowników
            var role_a = roleManager.FindByName("Admin");
            if (role_a == null)
            {
                var roleresult = roleManager.Create(new IdentityRole("Admin"));
            }
            var role_m = roleManager.FindByName("Manager");
            if (role_m == null)
            {
                var roleresult = roleManager.Create(new IdentityRole("Manager"));
            }
            var role_r = roleManager.FindByName("Receptionist");
            if (role_r == null)
            {
                var roleresult = roleManager.Create(new IdentityRole("Receptionist"));
            }

            //var user = userManager.FindByName(name);
            //if (user == null)
            //{
            //    user = new ApplicationUser { UserName = name, Email = name };
            //    var result = userManager.Create(user, password);
            //    result = userManager.SetLockoutEnabled(user.Id, false);
            //}

            // Add user admin to Role Admin if not already added
            var rolesForUser_a = userManager.GetRoles(user_a.Id);
            var rolesForUser_m = userManager.GetRoles(user_m.Id);
            var rolesForUser_r = userManager.GetRoles(user_r.Id);
            if (!rolesForUser_a.Contains(role_a.Name))
            {
                var result = userManager.AddToRole(user_a.Id, role_a.Name);
            }
            if (!rolesForUser_m.Contains(role_m.Name))
            {
                var result = userManager.AddToRole(user_m.Id, role_m.Name);
            }
            if (!rolesForUser_r.Contains(role_r.Name))
            {
                var result = userManager.AddToRole(user_r.Id, role_r.Name);
            }
        }
    }
}