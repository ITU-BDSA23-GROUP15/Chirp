using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using SimpleDB;

class Program 
{
    public static void Main (string[] args)
    {
        IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>();
        if (args.Length > 0)
        {
            switch (args[0].ToLower())
            {
                case "read":
                    var list = database.Read();
                    foreach (var cheep in list)
                    {
                        Console.WriteLine($"{cheep.user} @ {UnixTimeStampToDateTime(cheep.timeStamp)}: {cheep.cheep}");
                    }
                    break;
                case "cheep":
                    if (args.Length > 1)
                    {
                        // Unix timestamp code adapted from: https://stackoverflow.com/questions/17632584/how-to-get-the-unix-timestamp-in-c-sharp
                        DateTime currentTime = DateTime.UtcNow;
                        long unixTime = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();
                        Cheep cheep = new Cheep(Environment.UserName, args[1], unixTime);
                        database.Store(cheep);
                    }
                    else
                    {
                        Console.WriteLine("Please provide a message for 'cheep' command.");
                    }
                    break;
                default:
                    Console.WriteLine("Please enter a valid command. Valid commands are: read, cheep");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Please enter a valid command. Valid commands are: read, cheep");
        }        
    }

    public record Cheep(string user, string cheep, long timeStamp)
    {
        public override string ToString()
        {
            return $"{user},{cheep},{timeStamp}";
        }
    }

    //code snatched from: https://stackoverflow.com/questions/249760/how-can-i-convert-a-unix-timestamp-to-datetime-and-vice-versa
    public static DateTime UnixTimeStampToDateTime(long unixTimeStamp )
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds( unixTimeStamp ).ToLocalTime();
        return dateTime;
    }
}
