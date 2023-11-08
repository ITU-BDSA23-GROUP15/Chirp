using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTests;

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
