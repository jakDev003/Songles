using Microsoft.EntityFrameworkCore;

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

    public class Song
    {
        public int Id { get; set; }
        public string? TrackName { get; set; }
        public string? Artist { get; set; }
        public string? Album { get; set; }
        public string? Genre { get; set; }
        public TimeOnly? Time { get; set; }
    }
}
