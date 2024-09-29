using Microsoft.EntityFrameworkCore;
using songles.Data;

await using var db = new Model();

Console.WriteLine($"Database path: {db.DbPath}.");

if (await db.Database.EnsureCreatedAsync())
{
    await Utilities.InitializeDatabase(db, "Data/songles.csv");
    Console.WriteLine("Database created.");
}
else
{
    Console.WriteLine("Database already exists.");
}

Console.WriteLine("Querying for songs");

var songs = from song in db.Songs
            where song.Artist == "Bad"
            && song.TrackName == "Shirts"
            select song;

foreach (var song in songs)
{
    Console.WriteLine($"Song: {song.TrackName} by {song.Artist}");
}