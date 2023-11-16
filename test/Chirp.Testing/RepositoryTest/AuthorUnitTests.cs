using System.Runtime.InteropServices;
using Chirp.Core;
using Microsoft.EntityFrameworkCore;

namespace Application.Testing;

public class AuthorUnitTests : BaseIntegrationTest
{
	private readonly AuthorRepository authorRepository;
	private readonly Faker<CreateAuthorDto> authorGenerator;

	public AuthorUnitTests(IntegrationTestWebAppFactory factory) : base(factory)
	{
		authorRepository = new AuthorRepository(DbContext);

		authorGenerator = new Faker<CreateAuthorDto>()
			.RuleFor(u => u.Name, (f, u) => f.Name.LastName())
			.RuleFor(u => u.Email, (f, u) => f.Internet.Email());
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
		await authorRepository.CreateAuthor(author);

		// Act
		// Assert
		await Assert.ThrowsAsync<Exception>(() => authorRepository.CreateAuthor(new CreateAuthorDto(author.Name, author.Email)));
	}

	[Fact]
	public async Task GetAuthorByName() {
		// Arrange
		var author = authorGenerator.Generate();
		await authorRepository.CreateAuthor(author);

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
		await authorRepository.CreateAuthor(author);

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