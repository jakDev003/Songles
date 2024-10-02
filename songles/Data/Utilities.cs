﻿using CsvHelper;
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
    }
}
