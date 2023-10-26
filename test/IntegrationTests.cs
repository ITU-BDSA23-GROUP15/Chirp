namespace test;

using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;


public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _fixture;
    private readonly HttpClient _client;
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<ChirpContext> _options;
    

    public IntegrationTests(WebApplicationFactory<Program> fixture)
    {
        _fixture = fixture;
        _client = _fixture.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = true, HandleCookies = true });
        
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        _options = new DbContextOptionsBuilder<ChirpContext>()
            .UseSqlite(_connection)
            .Options;
        
        var context = new ChirpContext(_options);
        context.Database.EnsureCreated();
    }

    [Fact]
    public async void CanSeePublicTimeline()
    {
        var response = await _client.GetAsync("/");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("Chirp!", content);
        Assert.Contains("Public Timeline", content);
    }

    [Theory]
    [InlineData("Helge")]
    [InlineData("Rasmus")]
    public async void CanSeePrivateTimeline(string author)
    {
        var response = await _client.GetAsync($"/{author}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("Chirp!", content);
        Assert.Contains($"{author}'s Timeline", content);
    }

    [Fact]
    public async void GetAuthorByName()
    {
        using (var context = new ChirpContext(_options))
        {
            context.Database.EnsureCreated();
            context.Authors.Add(new Author { Name = "testuser", Email = "testuser@gmail.com", Cheeps = new List<Cheep>() });
            await context.SaveChangesAsync();
        }
        using (var context = new ChirpContext(_options))
        {
            var repository = new AuthorRepository(context);
            var author = repository.GetAuthorByName("testuser");
            Assert.Equal("testuser", author.Result.Name);
        }
    }
}
