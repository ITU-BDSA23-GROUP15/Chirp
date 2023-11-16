using System.Runtime.InteropServices;
using Chirp.Core;
using Microsoft.EntityFrameworkCore;

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
			.RuleFor(u => u.Name, (f, u) => f.Name.LastName())
			.RuleFor(u => u.Email, (f, u) => f.Internet.Email());

		cheepGenerator = new Faker<Cheep>()
			.RuleFor(u => u.CheepId, (f, u) => f.Random.Guid())
			.RuleFor(u => u.Author, (f, u) => authorGenerator.Generate()) // generate new author
			.RuleFor(u => u.AuthorId, (f, u) => u.Author.AuthorId) // take fake author's id
			.RuleFor(u => u.Text, (f, u) => f.Lorem.Sentence()) // generate random sentence
			.RuleFor(u => u.TimeStamp, (f, u) => f.Date.Past()); // generate random date in the past
	}

	[Fact]
	public async Task CreateAuthorGetNameTest()
	{
		// Arrange
		var createAuthorDto = new CreateAuthorDto("JensPeter", "TestEmail@test.dk");
    	await authorRepository.CreateAuthor(createAuthorDto);

		// Act
		var author = await DbContext.Authors.FirstOrDefaultAsync(a => a.Name == createAuthorDto.Name);

		// Assert
		Assert.NotNull(author);
		Assert.Equal("JensPeter", author.Name);
	}

	[Fact]
	//Create test to throw AuthorAlreadyExistsException
	public async Task CreateAuthorThrowsException()
	{
		// Arrange
		var author = authorGenerator.Generate();
		await authorRepository.CreateAuthor(new CreateAuthorDto(author.Name, author.Email));

		// Act
		// Assert
		await Assert.ThrowsAsync<Exception>(() => authorRepository.CreateAuthor(new CreateAuthorDto(author.Name, author.Email)));
	}

	[Fact]
	public async Task GetAuthorByName() {
		// Arrange
		var author = authorGenerator.Generate();
		await authorRepository.CreateAuthor(new CreateAuthorDto(author.Name, author.Email));

		// Act
		var authorDto = await authorRepository.GetAuthorByName(author.Name);

		// Assert
		Assert.Equal(author.Name, authorDto.Name);
	}

	[Fact]
	public async Task GetAuthorByNameThrowsException() {
		// Arrange
		var author = authorGenerator.Generate();

		// Act

		// Assert
		await Assert.ThrowsAsync<Exception>(() => authorRepository.GetAuthorByName("WrongName"));
	}

	[Fact]
	public async Task GetAuthorByEmail() {
		// Arrange
		var author = authorGenerator.Generate();
		await authorRepository.CreateAuthor(new CreateAuthorDto(author.Name, author.Email));

		// Act
		var authorDto = await authorRepository.GetAuthorByEmail(author.Email);

		// Assert
		Assert.Equal(author.Email, authorDto.Email);
	}

	[Fact]
	public async Task GetAuthorByEmailThrowsException() {
		// Arrange
		var author = authorGenerator.Generate();

		// Act
		// Assert
		await Assert.ThrowsAsync<Exception>(() => authorRepository.GetAuthorByEmail("WrongEmail@fail.dk"));
	}

}