using songles.Service;
using songles.Data;
using System.Configuration;

bool debug = bool.TryParse(ConfigurationManager.AppSettings["Debug"], out var debugValue) && debugValue;

await using var db = new Model();

var songlesCSVPath = ConfigurationManager.AppSettings["SonglesCSVPath"];

if (string.IsNullOrEmpty(songlesCSVPath))
{
    Console.WriteLine("Please set the SonglesCSVPath in the app.config file.");
    return;
}

if (await db.Database.EnsureCreatedAsync())
{
    await Utilities.InitializeDatabase(db, songlesCSVPath);

    if (debug)
    {
        Console.WriteLine($"Database path: {db.DbPath}.");
        Console.WriteLine("Database created.");
        Console.WriteLine();
    }
}

var dj = new DiskJockey(db, debug);
var userInput = new UserInput(TimeSpan.FromSeconds(1), dj);
userInput.Start();
dj.Play();
