
// using System.Data;
// using Microsoft.Data.Sqlite;

// namespace Chirp.Razor;

// public class DBFacade {
//     private readonly string sqlDBFilePath;

//     public DBFacade(string sqlDBFilePath) 
//     {
//         sqlDBFilePath = "/mnt/c/Users/mchrn/ITU/3_Semester/Chirp/src/Chirp.SQLite/data/chirp.db";
//     }

//     // public DBConnection CreateConnection()
//     // {
//     //     return new SqlConnection(sqlDBFilePath);
//     // }

//     public List<CheepViewModel> GetCheeps()
//     {
//         List<CheepViewModel> cheeps = new List<CheepViewModel>();

//         using (var connection = new SqlConnection("/mnt/c/Users/mchrn/ITU/3_Semester/Chirp/src/Chirp.SQLite/data/chirp.db"))
//         {
//             connection.Open();

//             using (var command = connection.CreateCommand())
//             {
//                 command.CommandText = @"SELECT * FROM message ORDER by message.pub_date desc";
//                 using (var reader = command.ExecuteReader())
//                 {
//                     while (reader.Read())
//                     {
//                         cheeps.Add(new CheepViewModel
//                         {
//                             Id = reader.GetInt32(0),
//                             Username = reader.GetString(1),
//                             Message = reader.GetString(2),
//                             Timestamp = reader.GetDateTime(3)
//                         });

//                         cheeps.Add(cheep);
//                     }
//                 }
//             }
//         }

//         return cheeps;
//     }

//     public void AddCheep(CheepViewModel cheep)
//     {
//         using (var connection = new SqlConnection("/mnt/c/Users/mchrn/ITU/3_Semester/Chirp/src/Chirp.SQLite/data/chirp.db"))
//         {
//             connection.Open();

//             using (var command = connection.CreateCommand())
//             {
//                 command.CommandText = "INSERT INTO Cheeps (Username, Message, Timestamp) VALUES (@Username, @Message, @Timestamp)";
//                 command.Parameters.AddWithValue("@Username", cheep.Username);
//                 command.Parameters.AddWithValue("@Message", cheep.Message);
//                 command.Parameters.AddWithValue("@Timestamp", cheep.Timestamp);

//                 command.ExecuteNonQuery();
//             }
//         }
//     }
// }


using System.Data;
using Microsoft.Data.Sqlite;
using Chirp.CheepService;
public class DBFacade {
    private string sqlDBFilePath = "/mnt/c/Users/mchrn/ITU/3_Semester/Chirp/src/Chirp.Razor/data/chirp.db";
    public List<CheepViewModel> GetCheeps() 
    {
        List<CheepViewModel> cheeps = new List<CheepViewModel>();

        var sqlQuery = @"SELECT * FROM message JOIN user u ON u.user_id = message.author_id ORDER by message.pub_date desc";

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