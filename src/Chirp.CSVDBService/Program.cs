var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


var database = CSVDatabase.CSVDatabase<Cheep>.Instance;
database.filename = @"data/chirp_cli_db.csv";

app.MapGet("/cheeps", () => {
    return database.Read();
});

app.MapPost("/cheep", async (Cheep cheep) =>
{
    
    database.Store(cheep);
}); 

app.Run();

public record Cheep(string Author, string Message, long Timestamp);