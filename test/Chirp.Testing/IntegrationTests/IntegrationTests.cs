using Chirp.Core;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Application.Testing;

public class IntegrationTests : BaseIntegrationTest
{
	private readonly HttpClient _client;
	private readonly IntegrationTestWebAppFactory _factory;

	public IntegrationTests(IntegrationTestWebAppFactory factory) : base(factory)
	{
		_factory = factory;
		_client = factory.CreateClient(new WebApplicationFactoryClientOptions {
			AllowAutoRedirect = false
		});

	}

	[Fact]
    public void EnsureDatabaseIsCreated()
    {
		// Check comes from: https://stackoverflow.com/a/69811634
		Assert.True(DbContext.Database.GetService<IRelationalDatabaseCreator>().HasTables());
    }

	// Below does not work currently
	// [Fact]
	// public async Task pingRequest_returnsPong() {

	// 	var response = await _client.GetAsync("/ping");

	// 	response.EnsureSuccessStatusCode();
	// 	var content = await response.Content.ReadAsStringAsync();
	// 	Assert.Equal("pong", content);
	// }
}