using CSVDBService;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

IDatabaseRepository<Cheep> database = CSVDatabase.CSVDatabase<Cheep>.Instance(@"data/chirp_cli_db.csv");

app.MapGet("/", () => {
    return;
});

app.MapGet("/cheeps", () => {
    return database.Read();
});

app.MapPost("/cheep", (Cheep cheep) =>
{
    database.Store(cheep);
}); 

app.Run();

public record Cheep(string Author, string Message, long Timestamp);