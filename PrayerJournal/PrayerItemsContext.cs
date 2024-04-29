using Microsoft.EntityFrameworkCore;

namespace PrayerJournal
{
    public class PrayerItemsContext : DbContext
    {
        public DbSet<PrayerItem> PrayerItems { get; set; }

        public string DbPath { get; }

        public PrayerItemsContext()
        {
            //var folder = Environment.SpecialFolder.LocalApplicationData;
            //var path = Environment.GetFolderPath(folder);
            
            //DbPath = System.IO.Path.Join(path, "prayer_journal.db");

            DbPath = @"c:\temp\prayer_journal.db";
            
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}

//https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli
