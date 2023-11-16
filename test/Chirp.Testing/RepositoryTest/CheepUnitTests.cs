using Chirp.Core;

namespace Application.Testing;

public class CheepUnitTests : BaseIntegrationTest
{
	private readonly CheepRepository cheepRepository;
	private readonly AuthorRepository authorRepository;
	private readonly Faker<CreateAuthorDto> authorGenerator;
	private readonly Faker<CreateCheepDto> cheepGenerator;
	public CheepUnitTests(IntegrationTestWebAppFactory factory) : base(factory)
	{
		cheepRepository = new CheepRepository(DbContext);
		authorRepository = new AuthorRepository(DbContext);

		authorGenerator = new Faker<CreateAuthorDto>()
			.RuleFor(u => u.Name, (f, u) => f.IndexGlobal + f.Internet.UserName()) //Index is to ensure that the name is unique
			.RuleFor(u => u.Email, (f, u) => f.Internet.Email());

		cheepGenerator = new Faker<CreateCheepDto>()
			.RuleFor(u => u.Text, (f, u) => "test sentence")
			.RuleFor(u => u.Author, (f, u) => authorGenerator.Generate().Name);
	}

	[Fact]
	public void CreateCheepGetTextTest()
	{
		// Arrange
		DbContext.RemoveRange(DbContext.Cheeps); //Removes all Cheeps made in former tests
		DbContext.RemoveRange(DbContext.Authors); //Removes all Authors made in former tests

		// Act
		var cheep = cheepGenerator.Generate();

		// Assert
		Assert.Equal("test sentence", cheep.Text);
	}

	[Fact]
	public async Task GetCheepsReturn1Cheep()
	{
		// Arrange
		DbContext.RemoveRange(DbContext.Cheeps); //Removes all Cheeps made in former tests
		DbContext.RemoveRange(DbContext.Authors); //Removes all Authors made in former tests

		var fakeAuthor = authorGenerator.Generate();

		await authorRepository.CreateAuthor(fakeAuthor);
		await cheepRepository.CreateCheep(new CreateCheepDto(cheepGenerator.Generate().Text!, fakeAuthor.Name!));

		// Act
		var cheeps = await cheepRepository.GetCheeps(1, 32); // first page, 32 cheeps taken

		// Assert
		Assert.Single(cheeps.ToList());
	}

	[Fact]
	public async Task GetCheepsReturn1CheepFromAuthor()
	{
		// Arrange
		DbContext.RemoveRange(DbContext.Cheeps); //Removes all Cheeps made in former tests
		DbContext.RemoveRange(DbContext.Authors); //Removes all Authors made in former tests

		var specificAuthor = authorGenerator.Generate();

		await authorRepository.CreateAuthor(specificAuthor);
		await cheepRepository.CreateCheep(new CreateCheepDto(cheepGenerator.Generate().Text!, specificAuthor.Name!));

		foreach (var fakeCheep in cheepGenerator.Generate(10))
		{
			var fakeAuthor = authorGenerator.Generate();
			await authorRepository.CreateAuthor(fakeAuthor);
			await cheepRepository.CreateCheep(new CreateCheepDto(fakeCheep.Text!, fakeAuthor.Name!));
		}

		// Act
		var cheeps = await cheepRepository.GetCheepsFromAuthor(specificAuthor.Name!, 1, 32); // first page, 32 cheeps taken

		// Assert
		Assert.Single(cheeps);
	}

	[Fact]
	public async Task GetCheepsReturn10Cheeps()
	{
		// Arrange
		DbContext.RemoveRange(DbContext.Cheeps); //Removes all Cheeps made in former tests
		DbContext.RemoveRange(DbContext.Authors); //Removes all Authors made in former tests

		var fakeAuthor = authorGenerator.Generate();
		await authorRepository.CreateAuthor(fakeAuthor);

		foreach (var fakeCheep in cheepGenerator.Generate(10))
		{
			await cheepRepository.CreateCheep(new CreateCheepDto(fakeCheep.Text!, fakeAuthor.Name!));
		}

		// Act
		var cheeps = await cheepRepository.GetCheeps(1, 32); // first page, 32 cheeps taken

		// Assert
		Assert.Equal(10, cheeps.Count());
	}
}