using Microsoft.EntityFrameworkCore;
using songles.Data.Models;

namespace songles.Data
{
    internal class Model : DbContext
    {
        public DbSet<Song> Songs { get; set; }
        public string DbPath { get; private set; }

        public Model()
        {
            DbPath = "songles.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }

    }

    
}
