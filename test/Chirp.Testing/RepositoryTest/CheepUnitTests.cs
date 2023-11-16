using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Testing;

public class CheepUnitTests : BaseIntegrationTest
{
	CheepRepository cheepRepository;
	public CheepUnitTests(IntegrationTestWebAppFactory factory) : base(factory)
	{
		cheepRepository = new CheepRepository(DbContext);
	}

	[Fact]
    public void EnsureDatabaseIsCreated()
    {
		// Check comes from: https://stackoverflow.com/a/69811634
		Assert.True(DbContext.Database.GetService<IRelationalDatabaseCreator>().HasTables());
    }

	[Fact]
	public async Task CreateCheep()
	{
		var fakeAuthor = new Faker<Author>;
		await cheepRepository.CreateCheep(cheep);
		var cheeps = await cheepRepository.GetCheeps(1, 32);
		Assert.Equal(1, cheeps.Count());
	}

}