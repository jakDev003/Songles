using CsvHelper;
using songles.Data.Models;
using System.Globalization;

namespace songles.Data
{
    internal static class Utilities
    {

        /// <summary>
        /// Intialized the database with the data from the csv file
        /// </summary>
        /// <param name="db"></param>
        /// <param name="pathToFile"></param>
        /// <returns></returns>
        public static async Task InitializeDatabase(Model db, string pathToFile)
        {
            using (var reader = new StreamReader(pathToFile))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<CsvSong>();
                foreach (var record in records)
                {
                    /// adjust the time to the correct format
                    /// by adding the value for hours to by used
                    /// in the TimeOnly constructor
                    TimeOnly timeToAdd;
                    if (record.Time != null)
                    {
                        var time = record.Time.Value.ToString().Split(':');
                        timeToAdd = new TimeOnly(0, int.Parse(time[0]), int.Parse(time[1].Split(' ')[0]));
                    }
                    else
                    {
                        timeToAdd = new TimeOnly(0, 0, 0);
                    }

                    /// add the song to the database
                    var songToAdd = new Song
                    {
                        TrackName = record.TrackName,
                        Artist = record.Artist,
                        Album = record.Album,
                        Genre = record.Genre,
                        Time = timeToAdd
                    };
                    await db.Songs.AddAsync(songToAdd);
                }
                await db.SaveChangesAsync();
            }
        }
    }
}
