using System.Globalization;
using CSVDBService;
using CsvHelper;
using CsvHelper.Configuration;

namespace CSVDatabase;
public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{

    // Singleton pattern from https://csharpindepth.com/Articles/Singleton
    private static readonly Lazy<CSVDatabase<T>> lazy = 
        new Lazy<CSVDatabase<T>>(() => new CSVDatabase<T>());

    public static CSVDatabase<T> Instance { get { return lazy.Value; } }

    private CSVDatabase()
    { 
    }
    public string? filename;

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
