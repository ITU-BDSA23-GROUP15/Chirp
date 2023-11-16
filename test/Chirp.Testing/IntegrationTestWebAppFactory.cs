using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Testing;

// Code is adapted from: https://www.milanjovanovic.tech/blog/testcontainers-integration-testing-using-docker-in-dotnet
public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
	private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
		.WithImage("mcr.microsoft.com/mssql/server:2022-latest")
		.WithPassword("Strong_Password_123!")
		.Build();
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.UseEnvironment("Testing");
		builder.ConfigureTestServices(services =>
		{
			var descriptorType =
				typeof(DbContextOptions<ChirpContext>);

			var descriptor = services
			.SingleOrDefault(s => s.ServiceType == descriptorType);

			if (descriptor != null)
			{
				services.Remove(descriptor);
			}

			services.AddDbContext<ChirpContext>(options =>
				options.UseSqlServer(_dbContainer.GetConnectionString()));		 
		});
	}
	public Task InitializeAsync()
	{
		return _dbContainer.StartAsync();
	}

	public new Task DisposeAsync()
	{
		return _dbContainer.StopAsync();
	}
}
