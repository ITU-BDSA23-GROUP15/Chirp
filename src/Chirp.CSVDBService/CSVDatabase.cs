using System.Globalization;
using CSVDBService;
using CsvHelper;
using CsvHelper.Configuration;

namespace CSVDatabase;
public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{

    // Singleton pattern from https://csharpindepth.com/Articles/Singleton
    private static CSVDatabase<T>? instance = null;
    private static readonly object padlock = new object();

    public static CSVDatabase<T> Instance(string filename) 
    {
        lock (padlock)
        {
            if(instance == null)
            {
                instance = new CSVDatabase<T>(filename);
            }
            return instance;
        }
    }

    CSVDatabase(string filename)
    { 
        this.filename = filename;
    }
    public string filename;

    public void Store(T record)
    {
        using (StreamWriter writer = new StreamWriter(filename, true)) // boolean true means append, false means overwrite
        {
            writer.WriteLine(record);
        }
    }

    public IEnumerable<T> Read(int? limit = 10)
    {
        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture);

        using (StreamReader reader = new StreamReader(filename))
        using (CsvReader csv = new CsvReader(reader, csvConfig))
        return csv.GetRecords<T>().Take(limit ?? 10).ToList();
    }
}
