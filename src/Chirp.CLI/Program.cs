using SimpleDB;
using static Chirp.UserInterface;

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
                    PrintCheeps(list);
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
}
