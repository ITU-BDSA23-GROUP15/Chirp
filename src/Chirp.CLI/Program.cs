using static Chirp.UserInterface;
using System.CommandLine;
using System.Net.Http.Headers;
using System.Net.Http.Json;

var baseURL = "https://bdsagroup15chirpremotedb.azurewebsites.net";
using HttpClient client = new();
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
client.BaseAddress = new Uri(baseURL);

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

readCommand.SetHandler(async (readArgValue) =>
{
   var cheeps = await client.GetFromJsonAsync<IEnumerable<Cheep>>("/cheeps");
    PrintCheeps(cheeps.Take<Cheep>(readArgValue));
}, readArg);

cheepCommand.SetHandler(async (cheepArgValue) =>
{
    DateTime currentTime = DateTime.UtcNow;
    long unixTime = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();
    Cheep cheep = new Cheep(Environment.UserName, cheepArgValue, unixTime);
            
    HttpResponseMessage response = await client.PostAsJsonAsync<Cheep>("/cheep", cheep);

    if((int) response.StatusCode >= 400)
    {
        Console.WriteLine($"POST went wrong ({response.StatusCode})");
    }
    else
    {
        Console.WriteLine($"POST succesful ({response.StatusCode})");
    }
}, cheepArg);

await rootCommand.InvokeAsync(args);

public record Cheep(string Author, string Message, long Timestamp)
{
    public override string ToString()
    {
        return $"{Author},{Message},{Timestamp}";
    }
}
