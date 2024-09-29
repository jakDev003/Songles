using songles.Controller;
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

var dj = new DiskJockey(db);
dj.Play();