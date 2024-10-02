using CsvHelper;
using songles.Data.Models;
using System.Globalization;

namespace songles.Data
{
    internal static class Utilities
    {
        const char _block = '■';

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
                    var songToAdd = new Song
                    {
                        TrackName = record.TrackName,
                        Artist = record.Artist,
                        Album = record.Album,
                        Genre = record.Genre,
                        Time = record.Time
                    };
                    await db.Songs.AddAsync(songToAdd);
                }
                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Returns a string of progress bars based on the current and total values
        /// </summary>
        /// <param name="total"></param>
        /// <param name="elapsedTime"></param>
        /// <param name="totalTime"></param>
        /// <param name="current"></param>

        /// <returns></returns>
        public static void SetProgressBar(int percent, TimeSpan elapsedTime, TimeOnly totalTime, bool update = false)
        {
            if (update)
                Console.Write("\r");
            Console.Write("[");
            var p = (int)((percent / 20f) + .5f);
            for (var i = 0; i < 20; ++i)
            {
                if (i >= p)
                    Console.Write(' ');
                else
                    Console.Write(_block);
            }
            Console.Write("] {0,3:##0}% ", percent);
            Console.Write($"({TimeSpanToString(elapsedTime)}/{TimeOnlyToString(totalTime)})");
        }

        /// <summary>
        /// TimeOnly to string
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private static string TimeOnlyToString(TimeOnly time)
        {
            return $"{time.Hour}:{time.Minute}:{time.Second}";
        }

        /// <summary>
        /// TimeSpan to string
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private static string TimeSpanToString(TimeSpan time)
        {
            return $"{time.Hours}:{time.Minutes}:{time.Seconds}";
        }
    }

    
}
