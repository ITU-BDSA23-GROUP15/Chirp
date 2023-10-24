using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Set connection string
var folder = Environment.SpecialFolder.LocalApplicationData;
var path = Environment.GetFolderPath(folder);
string connectionString = Path.Join(path, "chirp.db");
Console.WriteLine(connectionString);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ChirpContext>(options => options.UseSqlite($"Data source={connectionString}"));
builder.Services.AddScoped<ICheepRepository, CheepRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope()){
    
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ChirpContext>();
    DbInitializer.SeedDatabase(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();

public partial class Program {}
