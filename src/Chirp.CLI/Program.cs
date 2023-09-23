using System.CommandLine;
using CSVDBService;
using CSVDatabase;
using static Chirp.UserInterface;

public class Program
{
    static async Task Main(string[] args)
    {
        var database = CSVDatabase<Cheep>.Instance;
        database.filename = Environment.CurrentDirectory + @"/data/chirp_cli_db.csv";

        var rootCommand = new RootCommand("Chirp command-line app");

        var readCommand = new Command("read", "Reading cheeps");
        var readArg = new Argument<int>
            (name: "readNumber",
            description: "Number of cheeps to read",
            getDefaultValue: () => 10);
        rootCommand.AddCommand(readCommand);
        readCommand.Add(readArg);

        var cheepCommand = new Command("cheep", "Write cheep");
        var cheepArg = new Argument<string>
            (name: "message",
            description: "Message to cheep");
        rootCommand.AddCommand(cheepCommand);
        cheepCommand.Add(cheepArg);

        readCommand.SetHandler((readArgValue) =>
        {
            var list = database.Read();
            PrintCheeps(list);
        }, readArg);

        cheepCommand.SetHandler((cheepArgValue) =>
        {
            DateTime currentTime = DateTime.UtcNow;
            long unixTime = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();
            Cheep cheep = new Cheep(Environment.UserName, cheepArgValue, unixTime);
            database.Store(cheep);
        }, cheepArg);

        await rootCommand.InvokeAsync(args);
    }

public static string FormatDateTime(long timeStamp)
{
    var formattedDateTime = UnixTimeStampToDateTime(timeStamp).ToString("dd.MM.yyyy HH.mm.ss");
    return formattedDateTime;
}
public record Cheep(string User, string Message, long TimeStamp)
{
    public override string ToString()
    {
        return $"{User},{Message},{TimeStamp}";
    }
}


}
