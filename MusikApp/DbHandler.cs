using Microsoft.Data.Sqlite;

namespace MusikApp;

/// <summary>
/// ALL databaskod bor här. Program.cs ska aldrig innehålla SQL eller SqliteConnection.
///
/// Det här är ett skelett: metoderna finns, kommentarerna visar vad varje metod ska göra,
/// men koden är din. Alla metoder kastar NotImplementedException tills du har skrivit dem.
///
/// ADO.NET i fem steg (samma i varje metod som pratar med databasen):
///   1. Öppna en anslutning        -> new SqliteConnection(...) och connection.Open()
///   2. Skapa ett kommando         -> connection.CreateCommand()
///   3. Skriv SQL:en               -> command.CommandText = "..."
///   4. Skicka in värden säkert    -> command.Parameters.AddWithValue("@namn", värde)
///   5. Kör kommandot              -> ExecuteReader() för SELECT, ExecuteNonQuery() för INSERT/DELETE
///
/// Använd "using var" på anslutning och kommando så stängs de av sig själva.
/// Bygg SQL:en med parametrar (@titel), aldrig genom att klistra ihop strängar.
/// </summary>
public class DbHandler : IDisposable
{
    public string connectionString { get; private set; }
    private SqliteConnection connection { get; }

    public DbHandler(string connectionString)
    {
        this.connectionString = connectionString;
        connection = new SqliteConnection(connectionString);
        connection.Open();
    }

    public void Dispose()
    {
        connection.Dispose();
    }
    // ---------------------------------------------------------------
    // G-nivå: du behöver minst tre av de fem här (välj själv vilka)
    // ---------------------------------------------------------------

    /// <summary>Hämtar alla låtar, sorterade på titel.</summary>
    public List<Song> GetAllSongs()
    {
        var songs = new List<Song>();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM songs ORDER BY titel";
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var song = new Song
            {
                Id = reader.GetInt32(0),
                Titel = reader.GetString(1),
                Length = reader.GetInt32(2),
                AlbumId =reader.GetInt32(3)

            };
            songs.Add(song);
        }
        return songs;
    }

    /// <summary>Hämtar en specifik låt. Returnerar null om den inte finns.</summary>
    public Song? GetSongById(int id)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM songs where id = @id;";
        command.Parameters.AddWithValue("@id", id);
        using var reader = command.ExecuteReader();
        if (reader.Read())
    
        {
            var song = new Song
            {
                Id = reader.GetInt32(0),
                Titel = reader.GetString(1),
                Length = reader.GetInt32(2),
                AlbumId =reader.GetInt32(3)

            };
            return song;
        }
        else
        {
            return null;
        }
       
    }

    /// <summary>Söker låtar där titeln innehåller söktexten.</summary>
    public List<Song> SearchSongs(string searchText)
    {
        // TODO: SELECT ... WHERE Titel LIKE @sok.
        // Tips: parametervärdet ska vara "%" + searchText + "%" så att LIKE matchar delar av titeln.
        throw new NotImplementedException();
    }

    /// <summary>Lägger till en ny låt.</summary>
    public void AddSong(Song song)
    {
        using var command = connection.CreateCommand();

        command.CommandText = "INSERT INTO songs  (titel, length, album_id)  VALUES (@titel,@length,@albumId)";

        command.Parameters.AddWithValue("@titel", song.Titel);
        command.Parameters.AddWithValue("@length", song.Length);
        command.Parameters.AddWithValue("@albumId", song.AlbumId);
        // Lägg till parametrarna för längd och album-id här.

        int addedRows = command.ExecuteNonQuery();
        Console.WriteLine($"{addedRows} låt tillagd.");
    }

    /// <summary>Tar bort en låt. Returnerar true om en rad faktiskt togs bort.</summary>
    public bool DeleteSong(int id)
    {
        // TODO: DELETE FROM ... WHERE Id = @id.
        // Tips: ExecuteNonQuery() returnerar antalet påverkade rader.
        // Menyn i Program.cs ska fråga "Är du säker? j/n" innan den här metoden anropas.
        using var command = connection.CreateCommand();

    command.CommandText = 
        """
        BEGIN TRANSACTION;

        DELETE FROM song_genres WHERE song_id = @id;
        DELETE FROM songs WHERE id = @id;

        COMMIT;
        """;

    command.Parameters.AddWithValue("@id", id);
    try
    {
        int deletedRows = command.ExecuteNonQuery();
        return deletedRows > 0;
    }
    catch
    {
        using var rollback = connection.CreateCommand();
        rollback.CommandText = "ROLLBACK;";
        rollback.ExecuteNonQuery();
        throw;
    }
}

    // ---------------------------------------------------------------
    // VG-nivå: du behöver minst EN av dessa två (och reflektionen i README)
    // ---------------------------------------------------------------

    /// <summary>(VG, alternativ A) Låtar tillsammans med albumets namn. Kräver en JOIN.</summary>
    public List<(string SongTitle, string AlbumName)> GetSongsWithAlbum()
    {
        // TODO: SELECT Låtar.Titel, Album.Namn FROM Låtar JOIN Album ON ... ORDER BY ...
        throw new NotImplementedException();
    }

    /// <summary>(VG, alternativ B) Låtar som är längre än medellängden. Kräver en subquery.</summary>
    public List<Song> GetSongsLongerThanAverage()
    {
        // TODO: SELECT ... WHERE Längd > (SELECT AVG(Längd) FROM Låtar).
        throw new NotImplementedException();
    }
}




