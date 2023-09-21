using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using SimpleDB;

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
        using var stream = File.Open(filename, FileMode.Append);
        using var writer = new StreamWriter(stream);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecord(record);
    }

    public IEnumerable<T> Read()
    {
        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture);

        using (StreamReader reader = new StreamReader(filename))
        using (CsvReader csv = new CsvReader(reader, csvConfig))
        return csv.GetRecords<T>().ToList();
    }
}