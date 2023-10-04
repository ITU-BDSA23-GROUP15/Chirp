using Microsoft.Data.Sqlite;
using Chirp.CheepService;
using System.Reflection;
using Microsoft.Extensions.FileProviders;
public class DBFacade {

    public DBFacade()
    {
        CreateDatabase();
    }

    private string connString;

    // https://stackoverflow.com/questions/34691378/creating-sqlite-database-using-dump-file-programmatically
    private void CreateDatabase()
    {
        var builder = new SqliteConnectionStringBuilder
        {
            DataSource = Path.GetTempPath() + "data.db",
        };
        connString = builder.ToString();

        // create schema
        ExecuteSQLQuery("schema.sql");

        // insert content
        ExecuteSQLQuery("dump.sql");
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

    // https://gist.github.com/kristopherjohnson/3229248
    private string ReadResourceFile(string filename) 
    {
        var embeddedProvider = new EmbeddedFileProvider(GetType().GetTypeInfo().Assembly, "Chirp.Razor.data");
        var files = embeddedProvider.GetDirectoryContents("");
        using var reader = embeddedProvider.GetFileInfo(filename).CreateReadStream();
        using var sr = new StreamReader(reader);
        return sr.ReadToEnd();
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
