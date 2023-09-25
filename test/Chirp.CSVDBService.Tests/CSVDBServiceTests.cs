namespace Chirp.CSVDBService.Tests;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using CSVDBService;

public class CSVDBServiceTests 
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public CSVDBServiceTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetCheeps_ReturnsSuccessStatusCode()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/cheeps");

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
    }
}
