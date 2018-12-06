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
                new Setting(){ SettingId = 1, Name = "NazwaPensjonatu", Value = "Sleep Well" },
                new Setting(){ SettingId = 2, Name = "MasterAdminId", Value = "" }
            };

            settings.ForEach(s => context.Settings.AddOrUpdate(s));
            context.SaveChanges();
        }
    }
}