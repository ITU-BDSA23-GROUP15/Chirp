using CSVDBService;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

IDatabaseRepository<Cheep> database = CSVDatabase.CSVDatabase<Cheep>.Instance(@"../../../../chirp_cli_db.csv");

app.MapGet("/", () => {
    return "it works gg";
});

app.MapGet("/cheeps", () => {
    return database.Read();
});

app.MapPost("/cheep", (Cheep cheep) =>
{
    database.Store(cheep);
}); 

app.Run();

record Cheep(string Author, string Message, long Timestamp);
