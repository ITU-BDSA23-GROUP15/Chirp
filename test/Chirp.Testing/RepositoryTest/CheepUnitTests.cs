using Chirp.Core;

namespace Application.Testing;

public class CheepUnitTests : BaseIntegrationTest
{
	CheepRepository cheepRepository;
	Faker<Author> authorGenerator;
	public CheepUnitTests(IntegrationTestWebAppFactory factory) : base(factory)
	{
		cheepRepository = new CheepRepository(DbContext);

		authorGenerator = new Faker<Author>()
			.RuleFor(u => u.AuthorId, (f, u) => f.Random.Guid())
			.RuleFor(u => u.Name, (f, u) => f.Name.LastName())
			.RuleFor(u => u.Email, (f, u) => f.Internet.Email());
	}

	[Fact]
	public void CreateCheepGetTextTest()
	{
		// Arrange
		var cheepGenerator = new Faker<Cheep>()
			.RuleFor(u => u.CheepId, (f, u) => f.Random.Guid())
			.RuleFor(u => u.Author, (f, u) => authorGenerator.Generate()) // generate new author
			.RuleFor(u => u.AuthorId, (f, u) => u.Author.AuthorId) // take fake author's id
			.RuleFor(u => u.Text, (f, u) => "test sentence") // generate random sentence
			.RuleFor(u => u.TimeStamp, (f, u) => f.Date.Past()); // generate random date in the past

		// Act
		var cheep = cheepGenerator.Generate();

		// Assert
		Assert.Equal("test sentence", cheep.Text);
	}

	[Fact]
	public async Task GetCheepsReturn1Cheep()
	{
		// Arrange
		var cheepGenerator = new Faker<Cheep>()
			.RuleFor(u => u.CheepId, (f, u) => f.Random.Guid())
			.RuleFor(u => u.Author, (f, u) => authorGenerator.Generate()) // generate new author
			.RuleFor(u => u.AuthorId, (f, u) => u.Author.AuthorId) // take fake author's id
			.RuleFor(u => u.Text, (f, u) => f.Lorem.Sentence()) // generate random sentence
			.RuleFor(u => u.TimeStamp, (f, u) => f.Date.Past()); // generate random date in the past

		var cheep = cheepGenerator.Generate();
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
		var cheepGenerator = new Faker<Cheep>()
			.RuleFor(u => u.CheepId, (f, u) => f.Random.Guid())
			.RuleFor(u => u.Author, (f, u) => authorGenerator.Generate()) // generate new author
			.RuleFor(u => u.AuthorId, (f, u) => u.Author.AuthorId) // take fake author's id
			.RuleFor(u => u.Text, (f, u) => f.Lorem.Sentence()) // generate random sentence
			.RuleFor(u => u.TimeStamp, (f, u) => f.Date.Past()); // generate random date in the past

		var cheep = cheepGenerator.Generate();
		await cheepRepository.CreateCheep(new CreateCheepDto(cheep.Text, cheep.Author.Name));

		var listOfFakeCheeps = cheepGenerator.Generate(10);
		foreach (var fakeCheep in listOfFakeCheeps)
		{
			await cheepRepository.CreateCheep(new CreateCheepDto(fakeCheep.Text, fakeCheep.Author.Name));
		}

		// Act
		var cheeps = await cheepRepository.GetCheepsFromAuthor(cheep.Author.Name, 1, 32); // first page, 32 cheeps taken

		// Assert
		Assert.Single(cheeps);
	}

}