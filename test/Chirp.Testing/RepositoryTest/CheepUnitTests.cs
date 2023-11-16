using Chirp.Core;

namespace Application.Testing;

public class CheepUnitTests : BaseIntegrationTest
{
	private readonly CheepRepository cheepRepository;
	private readonly AuthorRepository authorRepository;
	private readonly Faker<Author> authorGenerator;
	private readonly Faker<Cheep> cheepGenerator;
	public CheepUnitTests(IntegrationTestWebAppFactory factory) : base(factory)
	{
		cheepRepository = new CheepRepository(DbContext);
		authorRepository = new AuthorRepository(DbContext);

		authorGenerator = new Faker<Author>()
			.RuleFor(u => u.AuthorId, (f, u) => f.Random.Guid())
			.RuleFor(u => u.Name, (f, u) => f.IndexGlobal + f.Name.LastName()) //Index is to ensure that the name is unique
			.RuleFor(u => u.Email, (f, u) => f.Internet.Email());

		cheepGenerator = new Faker<Cheep>()
			.RuleFor(u => u.CheepId, (f, u) => f.Random.Guid())
			.RuleFor(u => u.Author, (f, u) => authorGenerator.Generate()) // generate new author
			.RuleFor(u => u.AuthorId, (f, u) => u.Author.AuthorId) // take fake author's id
			.RuleFor(u => u.Text, (f, u) => "test sentence") // generate random sentence
			.RuleFor(u => u.TimeStamp, (f, u) => f.Date.Past()); // generate random date in the past
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

		var cheep = cheepGenerator.Generate();
		await authorRepository.CreateAuthor(new CreateAuthorDto(cheep.Author.Name, cheep.Author.Email));
		await cheepRepository.CreateCheep(new CreateCheepDto(cheep.Text, cheep.Author.Name));

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
		var cheep = cheepGenerator.Generate();
		await authorRepository.CreateAuthor(new CreateAuthorDto(cheep.Author.Name, cheep.Author.Email));
		await cheepRepository.CreateCheep(new CreateCheepDto(cheep.Text, cheep.Author.Name));

		
		var listOfFakeCheeps = cheepGenerator.Generate(10);
		foreach (var fakeCheep in listOfFakeCheeps)
		{
			System.Console.WriteLine(fakeCheep.Author.Name);
			await authorRepository.CreateAuthor(new CreateAuthorDto(fakeCheep.Author.Name, fakeCheep.Author.Email));
			await cheepRepository.CreateCheep(new CreateCheepDto(fakeCheep.Text, fakeCheep.Author.Name));
		}

		// Act
		var cheeps = await cheepRepository.GetCheepsFromAuthor(cheep.Author.Name, 1, 32); // first page, 32 cheeps taken

		// Assert
		Assert.Single(cheeps);
	}

	[Fact]
	public async Task GetCheepsReturn10Cheeps()
	{
		// Arrange
		DbContext.RemoveRange(DbContext.Cheeps); //Removes all Cheeps made in former tests
		DbContext.RemoveRange(DbContext.Authors); //Removes all Authors made in former tests

		var listOfFakeCheeps = cheepGenerator.Generate(10);
		foreach (var fakeCheep in listOfFakeCheeps)
		{
			await authorRepository.CreateAuthor(new CreateAuthorDto(fakeCheep.Author.Name, fakeCheep.Author.Email));
			await cheepRepository.CreateCheep(new CreateCheepDto(fakeCheep.Text, fakeCheep.Author.Name));
		}

		// Act
		var cheeps = await cheepRepository.GetCheeps(1, 32); // first page, 32 cheeps taken

		// Assert
		Assert.Equal(10, cheeps.Count());
	}

}