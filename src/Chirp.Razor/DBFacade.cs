namespace DBFacade;
using System.Data;
using Microsoft.Data.Sqlite;

public class DBFacade {
    private readonly string sqlDBFilePath;

    public DBFacade(string sqlDBFilePath) 
    {
        sqlDBFilePath = "/mnt/c/Users/mchrn/ITU/3_Semester/Chirp/src/Chirp.SQLite/data/chirp.db";
    }

    public DBConnection CreateConnection()
    {
        return new SqlConnection(sqlDBFilePath);
    }

    public List<CheepViewModel> GetCheeps()
    {
        List<CheepViewModel> cheeps = new List<CheepViewModel>();

        using (var connection = CreateConnection())
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT * FROM message ORDER by message.pub_date desc";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cheeps.Add(new CheepViewModel
                        {
                            Id = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            Message = reader.GetString(2),
                            Timestamp = reader.GetDateTime(3)
                        });

                        cheeps.Add(cheep);
                    }
                }
            }
        }

        return cheeps;
    }

    public void AddCheep(CheepViewModel cheep)
    {
        using (var connection = CreateConnection())
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Cheeps (Username, Message, Timestamp) VALUES (@Username, @Message, @Timestamp)";
                command.Parameters.AddWithValue("@Username", cheep.Username);
                command.Parameters.AddWithValue("@Message", cheep.Message);
                command.Parameters.AddWithValue("@Timestamp", cheep.Timestamp);

                command.ExecuteNonQuery();
            }
        }
    }
}

// // The following code is taken from https://github.com/itu-bdsa/lecture_notes/blob/main/sessions/session_05/Chirp.SQLite/Program.cs and adapted to our program
// var sqlDBFilePath = "/mnt/c/Users/mchrn/ITU/3_Semester/Chirp/src/Chirp.SQLite/data/chirp.db";
// // var sqlQuery = @"SELECT * FROM message ORDER by message.pub_date desc"; // Queries should be in CheepService.cs

// using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
// {
//     connection.Open();

//     var command = connection.CreateCommand();
//     command.CommandText = sqlQuery;

//     using var reader = command.ExecuteReader();
//     while (reader.Read())
//     {
//         // https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader?view=dotnet-plat-ext-7.0#examples
//         var dataRecord = (IDataRecord)reader;
//         for (int i = 0; i < dataRecord.FieldCount; i++)
//             Console.WriteLine($"{dataRecord.GetName(i)}: {dataRecord[i]}");

//         // See https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader.getvalues?view=dotnet-plat-ext-7.0
//         // for documentation on how to retrieve complete columns from query results
//         Object[] values = new Object[reader.FieldCount];
//         int fieldCount = reader.GetValues(values);
//         for (int i = 0; i < fieldCount; i++)
//             Console.WriteLine($"{reader.GetName(i)}: {values[i]}");
//     }
// }
