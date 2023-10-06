namespace test;

using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

public class UnitTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _fixture;
    private readonly HttpClient _client;

    public UnitTests(WebApplicationFactory<Program> fixture)
    {
        _fixture = fixture;
        _client = _fixture.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = true, HandleCookies = true });
    }

    [Fact]
    public async void TestHttpRequestForAuthorPath()
    {
        // Arrange
        string endpoint = "/Helge"; 

        // Act
        var response = await _client.GetAsync(endpoint);
        var htmlContent = await response.Content.ReadAsStringAsync();

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotEmpty(htmlContent);
    }

    [Fact]
    public async void TestHttpRequestForPagination()
    {
        // Arrange
        string endpoint = "/?page=2"; 
        
        // Act
        var response = await _client.GetAsync(endpoint);
        var htmlContent = await response.Content.ReadAsStringAsync();

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotEmpty(htmlContent);
    }

    [Fact]
    public async void TestHttpRequestForPaginationWithAuthor()
    {
        // Arrange
        string endpoint = "/Helge?page=2"; 
        
        // Act
        var response = await _client.GetAsync(endpoint);
        var htmlContent = await response.Content.ReadAsStringAsync();

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotEmpty(htmlContent);
    }
}