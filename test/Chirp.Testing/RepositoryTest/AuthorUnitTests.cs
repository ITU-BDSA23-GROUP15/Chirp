using Chirp.Core;

namespace Application.Testing;

public class AuthorUnitTests : BaseIntegrationTest
{
	private readonly CheepRepository cheepRepository;
	private readonly AuthorRepository authorRepository;
	private readonly Faker<Author> authorGenerator;
	private readonly Faker<Cheep> cheepGenerator;
	public AuthorUnitTests(IntegrationTestWebAppFactory factory) : base(factory)
	{
		cheepRepository = new CheepRepository(DbContext);
		authorRepository = new AuthorRepository(DbContext);

		authorGenerator = new Faker<Author>()
			.RuleFor(u => u.AuthorId, (f, u) => f.Random.Guid())
			.RuleFor(u => u.Name, (f, u) => "JensPeter")
			.RuleFor(u => u.Email, (f, u) => f.Internet.Email());

		cheepGenerator = new Faker<Cheep>()
			.RuleFor(u => u.CheepId, (f, u) => f.Random.Guid())
			.RuleFor(u => u.Author, (f, u) => authorGenerator.Generate()) // generate new author
			.RuleFor(u => u.AuthorId, (f, u) => u.Author.AuthorId) // take fake author's id
			.RuleFor(u => u.Text, (f, u) => f.Lorem.Sentence()) // generate random sentence
			.RuleFor(u => u.TimeStamp, (f, u) => f.Date.Past()); // generate random date in the past
	}

	[Fact]
	public void CreateAuthorGetNameTest()
	{
		// Arrange

		// Act
		var author = authorGenerator.Generate();

		// Assert
		Assert.Equal("JensPeter", author.Name);
	}

	[Fact]
	//Create test to throw AuthorAlreadyExistsException
	public async Task CreateAuthorThrowsAuthorAlreadyExistsException()
	{
		// Arrange
		var author = authorGenerator.Generate();
		await authorRepository.CreateAuthor(new CreateAuthorDto(author.Name, author.Email));

		// Act
		// Assert
		await Assert.ThrowsAsync<Exception>(() => authorRepository.CreateAuthor(new CreateAuthorDto(author.Name, author.Email)));
	}

}