using Microsoft.Data.Sqlite;
using Chirp.CheepService;
using System.Reflection;
public class DBFacade {

    public DBFacade()
    {
        CreateDatabase();
        Console.WriteLine(connString);
    }

    private string connString;
    private void CreateDatabase()
    {
        var builder = new SqliteConnectionStringBuilder
        {
            DataSource = Environment.CurrentDirectory + @"/data/data.db",
        };
        connString = builder.ToString();

        // create schema
        ExecuteSQLQuery("Chirp.Razor.data.schema.sql");

        // insert content
        ExecuteSQLQuery("Chirp.Razor.data.dump.sql");
    }

    private void ExecuteSQLQuery(string filename) 
    {
        using(var connection = new SqliteConnection(connString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            
            command.CommandText = ReadResourceFile(filename);

            command.ExecuteNonQuery();
        }
    }
    private string ReadResourceFile(string filename) 
    {
        //return Assembly.GetExecutingAssembly().GetManifestResourceNames(); // testing purpose
        var thisAssembly = Assembly.GetExecutingAssembly();
        using var stream = thisAssembly.GetManifestResourceStream(filename);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
    public List<CheepViewModel> GetCheeps(int offset = 1) 
    {
        List<CheepViewModel> cheeps = new List<CheepViewModel>();

        var sqlQuery = $@"SELECT * FROM message m
                            JOIN user u ON u.user_id = m.author_id 
                           ORDER by m.pub_date desc
                           LIMIT 32 OFFSET {offset}";

        using (var connection = new SqliteConnection(connString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                // https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader?view=dotnet-plat-ext-7.0#examples
                // https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader.getvalues?view=dotnet-plat-ext-7.0
                Object[] values = new Object[reader.FieldCount];
                reader.GetValues(values);
                cheeps.Add(new CheepViewModel(Author: $"{values[5]}", Message: $"{values[2]}", $"{values[3]}"));
            }
        }
        return cheeps;
    }
}
