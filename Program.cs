using System.Reflection;
using System.Text.RegularExpressions;

class Program 
{
    // The following block of code is inspired and adapted from: https://stackoverflow.com/questions/3507498/reading-csv-files-using-c-sharp/34265869#34265869
    public static void Main ()
    {
    string fileName = "/mnt/c/Users/mchrn/ITU/3_Semester/Chirp/chirp_cli_db.csv";
    string[] allCheeps;
        using (StreamReader reader = new StreamReader(fileName))
    {
        string line; 

        while ((line = reader.ReadLine()) != null)
        {
            //Define pattern
            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

            //Separating columns to array
            allCheeps = CSVParser.Split(line);

            /* Do something with X */
            Console.WriteLine(string.Join("\n", allCheeps));
            // Array.ForEach(allCheeps, Console.WriteLine);
        }
        
    }
    }
    // static void read(string[] allCheeps)
    // {
    //     Array.ForEach(allCheeps, Console.WriteLine); {
    //     }   
        
    // }
}
