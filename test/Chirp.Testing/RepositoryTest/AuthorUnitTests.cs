using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Testing;

public class AuthorUnitTests : BaseIntegrationTest
{
	public AuthorUnitTests(IntegrationTestWebAppFactory factory) : base(factory)
	{
	}

	[Fact]
    public void EnsureDatabaseIsCreated()
    {
		// Check comes from: https://stackoverflow.com/a/69811634
		Assert.True(DbContext.Database.GetService<IRelationalDatabaseCreator>().HasTables());
    }
}