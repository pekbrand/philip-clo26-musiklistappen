using Microsoft.Extensions.Configuration;
using MusikApp;
var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var connectionString = config.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Hittar ingen connection string 'Default' i appsettings.json.");

using var db = new DbHandler(connectionString);

// Console.WriteLine($"Musiklistappen: börja med att få menyn att fungera!  (databas: {connectionString})");
Console.Clear();
int getValidNumber(int min, int max)
{
    while (true)
    {
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Du skrev inget. Försök igen.");
            continue;
        }

        if (int.TryParse(input, out int number))
        {
            if (number >= min && number <= max)
            {
                return number;
            }
        }

        Console.WriteLine($"Ange ett heltal mellan {min} och {max}.");
    }
}

bool running = true;

while (running)
{
    Console.WriteLine("");
    Console.WriteLine("=== Musiklistappen ===");
    Console.WriteLine("1. Visa alla låtar");
    Console.WriteLine("2. Lägg till en låt");
    Console.WriteLine("3. Ta bort en låt");
    Console.WriteLine("4. Visa låtar längre än medellängden");
    Console.WriteLine("5. Avsluta");

    int option = getValidNumber(1,5);

    switch (option)
    {
        case 1:
            Console.Clear();
            Console.WriteLine("Du valde att visa alla låtar.");
            var songs = db.GetAllSongs();
            foreach (var song in songs)
            {
                Console.WriteLine($"{song.Id}: {song.Titel} ({song.Length} sek)");
            }
            break;

        case 2:
            Console.Clear();
            Console.WriteLine("Du valde att lägga till en låt.");
            Console.WriteLine("Ange Titel");
            string titel = Console.ReadLine() ?? "";
            Console.WriteLine("Ange längd i sekunder");
            int length = getValidNumber(1,999);
            Console.WriteLine("Ange album ID");
            int albumId = getValidNumber(1,999);
            var newSong = new Song
            {
                Titel = titel,
                Length = length,
                AlbumId = albumId
            };
            db.AddSong(newSong);
            break;
        case 3:
            Console.Clear();
            Console.WriteLine("Du valde att ta bort en låt.");
            var songsToDelete  = db.GetAllSongs();
            foreach (var song in songsToDelete )
            {
                Console.WriteLine($"{song.Id}: {song.Titel} ({song.Length} sek)");
            }
            Console.WriteLine("Ange id för låten du vill ta bort:");
            int songId = getValidNumber(1,int.MaxValue);
            var songToDelete = db.GetSongById(songId);
            if (songToDelete == null)
            {
                Console.WriteLine("Låten hittades inte.");
                break;
            }
            Console.WriteLine($"Vill du radera {songToDelete.Titel}? j/n");
            string? answer = Console.ReadLine();
            if (answer?.ToLower() == "j")
            {
                bool deleted = db.DeleteSong(songId);
                Console.WriteLine(deleted ? "Låten togs bort." : "Låten hittades inte.");
            }
            break;
        case 4:
            Console.Clear();
            Console.WriteLine("Låtar längre än medellängden:");
            var longSongs = db.GetSongsLongerThanAverage();
            foreach (var song in longSongs)
            {
                Console.WriteLine($"{song.Id}: {song.Titel} ({song.Length} sek)");
            }
            break;
        case 5:
            Console.WriteLine("Tack för idag!");
            running = false;
            break;
            
    }

}



// Tips på ordning (se UPPGIFT.md):
//   1. Rita upp huvudmenyn (alternativ numrerade 1-N) och läs in ett val med Console.ReadLine().
//      Skriv ut "Här kommer en lista senare" tills databasen är kopplad.
//   2. Skapa ett DbHandler-objekt: new DbHandler(connectionString)
//   3. Anropa DbHandlers metoder från menyn. Här ska ALDRIG SQL stå.
//   4. Avsluta snyggt när användaren väljer "Avsluta".
