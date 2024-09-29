using songles.Data.Enums;
using SQLite;
using System.ComponentModel.DataAnnotations;

namespace songles.Data.Models
{
    internal class Song
    {
        [Key, AutoIncrement]
        public int Id { get; set; }
        public string? TrackName { get; set; }
        public string? Artist { get; set; }
        public string? Album { get; set; }
        public string? Genre { get; set; }
        public TimeOnly? Time { get; set; }
        public SongState State { get; set; } = SongState.Unknown;
        public UserSongPreference UserPreference { get; set; } = UserSongPreference.None;
    }
}
