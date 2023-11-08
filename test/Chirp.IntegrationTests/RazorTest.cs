namespace Application.IntegrationTests;

public class RazorTest : BaseIntegrationTest
{
	private readonly HttpClient _client;
	public RazorTest(IntegrationTestWebAppFactory factory) : base(factory)
	{
		_client = factory.CreateClient();
	}

    [Fact]
    public async void CanSeePublicTimeline()
    {
        var response = await _client.GetAsync("/");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        System.Console.WriteLine(content);
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
		DbContext.Database.EnsureCreated();
		DbContext.Authors.Add(new Author { Name = "testuser", Email = "testuser@gmail.com", Cheeps = new List<Cheep>() });
		await DbContext.SaveChangesAsync();

		var repository = new AuthorRepository(DbContext);
		var author = repository.GetAuthorByName("testuser");
		Assert.Equal("testuser", author.Result.Name);
    }
}