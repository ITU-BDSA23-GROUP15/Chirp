using Microsoft.Data.Sqlite;
using Chirp.CheepService;
public class DBFacade {
    private string sqlDBFilePath = Environment.CurrentDirectory + @"/data/data.db";
    public List<CheepViewModel> GetCheeps(int offset = 1) 
    {
        List<CheepViewModel> cheeps = new List<CheepViewModel>();

        var sqlQuery = $@"SELECT * FROM message m
                            JOIN user u ON u.user_id = m.author_id 
                           ORDER by m.pub_date desc
                           LIMIT 32 OFFSET {offset}";

        using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                // https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader?view=dotnet-plat-ext-7.0#examples
                Object[] values = new Object[reader.FieldCount];
                reader.GetValues(values);
                cheeps.Add(new CheepViewModel(Author: $"{values[5]}", Message: $"{values[2]}", $"{values[3]}"));
                    //Console.WriteLine($"{dataRecord.GetName(i)}: {dataRecord[i]}");

                // // See https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader.getvalues?view=dotnet-plat-ext-7.0
                // // for documentation on how to retrieve complete columns from query results
                // Object[] values = new Object[reader.FieldCount];
                // int fieldCount = reader.GetValues(values);
                // for (int i = 0; i < fieldCount; i++)
                //     Console.WriteLine($"{reader.GetName(i)}: {values[i]}");
            }
        }
        return cheeps;
    }
}
