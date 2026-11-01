using Microsoft.Extensions.Configuration;
using MusikApp;
var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var connectionString = config.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Hittar ingen connection string 'Default' i appsettings.json.");

using var db = new DbHandler(connectionString);

Console.WriteLine($"Musiklistappen: börja med att få menyn att fungera!  (databas: {connectionString})");

int getValidNumber()
{
    while (true)
    {
        string? input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
        {
            Console.WriteLine("Your did not enter anything.");
            continue;
        }
        
        if(int.TryParse(input, out int number))
        { 
            if (number >= 1 && number <= 4)
            {
                return number;
            }
        } 
        Console.WriteLine("Please enter a number between 1 and 4");
    }
}

bool running = true;

while (running)
{

    Console.WriteLine("=== Musiklistappen ===");
    Console.WriteLine("1. Visa alla låtar");
    Console.WriteLine("2. Lägg till en låt");
    Console.WriteLine("3. Ta bort en låt");
    Console.WriteLine("4. Avsluta");

    int option = getValidNumber();

    switch (option)
    {
        case 1:
            Console.WriteLine("Du valde att visa alla låtar.");
            var songs = db.GetAllSongs();
            foreach (var song in songs)
            {
                Console.WriteLine($"{song.Id}: {song.Titel} ({song.Length} sek)");
            }
            break;

        case 2:
            Console.WriteLine("Du valde att lägga till en låt.");
            break;
        case 3:
            Console.WriteLine("Du valde att ta bort en låt.");
            break;

        case 4:
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
