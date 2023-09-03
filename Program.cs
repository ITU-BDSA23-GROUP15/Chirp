using System.Text.RegularExpressions;

class Program 
{
    // The following block of code is inspired and adapted from: https://stackoverflow.com/questions/3507498/reading-csv-files-using-c-sharp/34265869#34265869
    public static void Main ()
    {
        string fileName = Environment.CurrentDirectory + @"/chirp_cli_db.csv";
        string[] allCheeps;
        using (StreamReader reader = new StreamReader(fileName))
        {
            string line; 
            reader.ReadLine(); //Skip first line

            while ((line = reader.ReadLine()) != null)
            {
                //Define pattern
                Regex CSVParser = new Regex(",(?=(?:[^\"]*(?:\"[^\"]*\"))*[^\"]*$)");

                //Separating columns to array
                allCheeps = CSVParser.Split(line);
                string user = allCheeps[0];
                string cheep = allCheeps[1];
                string timeStamp = allCheeps[2];
                double unixTimeStamp = double.Parse(timeStamp);
                DateTime date = UnixTimeStampToDateTime(unixTimeStamp);
                string formattedTimeStamp = date.ToString("MM/dd/yy HH:mm:ss");


                /* Do something with X */
                Console.WriteLine(user + " @ " + formattedTimeStamp + ": " + cheep);
            }
        }
    }
    //code snatched from: https://stackoverflow.com/questions/249760/how-can-i-convert-a-unix-timestamp-to-datetime-and-vice-versa
    public static DateTime UnixTimeStampToDateTime(double unixTimeStamp )
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds( unixTimeStamp ).ToLocalTime();
        return dateTime;
    }
}
