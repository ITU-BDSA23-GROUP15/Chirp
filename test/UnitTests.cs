namespace test;

using System.Net;

public class UnitTests
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public UnitTests() {
        _baseUrl = "http://localhost:5273"; // https://bdsagroup15chirprazor.azurewebsites.net use this for azure

        _httpClient = new HttpClient() {
            BaseAddress = new Uri(_baseUrl)
        };
    }

    [Fact]
    public async void TestHttpRequestForAuthorPath()
    {
        // Arrange
        string endpoint = "/Helge"; 

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
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
        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
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
        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
        var htmlContent = await response.Content.ReadAsStringAsync();

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotEmpty(htmlContent);
    }
}