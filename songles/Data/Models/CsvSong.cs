namespace songles.Data.Models
{
    internal class CsvSong
    {
        public string? TrackName { get; set; }
        public string? Artist { get; set; }
        public string? Album { get; set; }
        public string? Genre { get; set; }
        public TimeOnly? Time { get; set; }
    }
}
