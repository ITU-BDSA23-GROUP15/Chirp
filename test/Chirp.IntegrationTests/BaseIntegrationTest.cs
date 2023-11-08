using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTests;

// Code is adapted from: https://www.milanjovanovic.tech/blog/testcontainers-integration-testing-using-docker-in-dotnet
public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>, IDisposable
{
	private readonly IServiceScope _scope;
	protected readonly ChirpContext DbContext;
	
	protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
	{
		_scope = factory.Services.CreateScope();
		DbContext = _scope.ServiceProvider
			.GetRequiredService<ChirpContext>();
	}
	public void Dispose()
	{
		_scope?.Dispose();
		DbContext?.Dispose();
	}
}
