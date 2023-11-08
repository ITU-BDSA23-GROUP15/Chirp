using Microsoft.AspNetCore.Mvc.Testing;

namespace Application.IntegrationTests;

public class RazorTests : BaseIntegrationTest
{
	private readonly HttpClient _client;
	public RazorTests(IntegrationTestWebAppFactory factory) : base(factory)
	{
		_client = factory.CreateClient(new WebApplicationFactoryClientOptions
		{
			AllowAutoRedirect = true,
			HandleCookies = true
		});
	}

	[Fact]
	public void AlwaysTrue()
	{
		Assert.True(true);
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
		DbContext.Database.EnsureCreated();
		DbContext.Authors.Add(new Author { Name = "testuser", Email = "testuser@gmail.com", Cheeps = new List<Cheep>() });
		await DbContext.SaveChangesAsync();

		var repository = new AuthorRepository(DbContext);
		var author = repository.GetAuthorByName("testuser");
		Assert.Equal("testuser", author.Result.Name);
    }
}