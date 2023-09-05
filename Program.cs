using System.Text.RegularExpressions;

class Program 
{
    public static void Main (string[] args)
    {
        string user = Environment.UserName;
        string fileName = Environment.CurrentDirectory + @"/chirp_cli_db.csv";
        
        
    if (args.Length > 0)
    {
        switch (args[0].ToLower())
        {
            case "read":
                ReadCheep(fileName);
                break;
            case "cheep":
                if (args.Length > 1)
                {
                    WriteCheep(user, args[1]);
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
    // The following block of code is inspired and adapted from: https://stackoverflow.com/questions/3507498/reading-csv-files-using-c-sharp/34265869#34265869
    public static void ReadCheep(string fileName) 
    {
        using (StreamReader reader = new StreamReader(fileName))
        {
            string line; 
            reader.ReadLine(); //Skip first line

            while ((line = reader.ReadLine()) != null)
            {
                Regex CSVParser = new Regex(",(?=(?:[^\"]*(?:\"[^\"]*\"))*[^\"]*$)"); //Define pattern
                string[] allCheeps = CSVParser.Split(line); //Separating columns to array
                string user = allCheeps[0];
                string cheep = allCheeps[1];
                string timeStamp = allCheeps[2];
                long unixTimeStamp = long.Parse(timeStamp);
                DateTime date = UnixTimeStampToDateTime(unixTimeStamp);
                string formattedTimeStamp = date.ToString("MM/dd/yy HH:mm:ss");

                Console.WriteLine($"{user} @ {formattedTimeStamp}: {cheep}");
            }
        }
    }
    // Unix timestamp code adapted from: https://stackoverflow.com/questions/17632584/how-to-get-the-unix-timestamp-in-c-sharp
    public static void WriteCheep(string user, string cheep)
    {
        string fileName = Environment.CurrentDirectory + @"/chirp_cli_db.csv";
        using (StreamWriter writer = new StreamWriter(fileName, true)) // boolean true means append, false means overwrite
        {
            DateTime currentTime = DateTime.UtcNow;
            long unixTime = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();
            writer.WriteLine(user + "," + cheep + "," + unixTime);
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
