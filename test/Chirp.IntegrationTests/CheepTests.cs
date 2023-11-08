namespace Application.IntegrationTests;

public class CheepTests : BaseIntegrationTest
{
	private readonly HttpClient _client;
	public CheepTests(IntegrationTestWebAppFactory factory) : base(factory)
	{
		_client = factory.CreateClient();
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
}