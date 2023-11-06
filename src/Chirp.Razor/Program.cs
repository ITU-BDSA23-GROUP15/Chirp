using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Set connection string
// var folder = Environment.SpecialFolder.LocalApplicationData;
// var path = Environment.GetFolderPath(folder);
// string connectionString = Path.Join(path, "chirp.db");
// Console.WriteLine(connectionString);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ChirpContext>(options => 
        options.UseSqlServer(builder.Configuration.GetConnectionString("ChirpDb")));
builder.Services.AddScoped<ICheepRepository, CheepRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
// Authentication with Azure AD B2C
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    using(var scope = app.Services.CreateScope())
    {
        /*
        creating database from SalesContext
        Making it from migrations another method is needed. For example: 
        
        dotnet ef database update --project SalesApi --startup-project SalesApi 
        */
        var chirpContext = scope.ServiceProvider.GetRequiredService<ChirpContext>(); 
        chirpContext.Database.EnsureCreated();
        DbInitializer.SeedDatabase(chirpContext);
    }
}

// using (var scope = app.Services.CreateScope())
// {

//     var services = scope.ServiceProvider;

//     var context = services.GetRequiredService<ChirpContext>();
//     DbInitializer.SeedDatabase(context);
// }

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCookiePolicy(new CookiePolicyOptions()
{ 
    Secure = CookieSecurePolicy.Always
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();

public partial class Program { }
