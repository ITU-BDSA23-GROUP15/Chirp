using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Testing;

public class IntegrationTests : BaseIntegrationTest
{
	public IntegrationTests(IntegrationTestWebAppFactory factory) : base(factory)
	{
	}

	[Fact]
    public void EnsureDatabaseIsCreated()
    {
		// Check comes from: https://stackoverflow.com/a/69811634
		Assert.True(DbContext.Database.GetService<IRelationalDatabaseCreator>().HasTables());
    }
}