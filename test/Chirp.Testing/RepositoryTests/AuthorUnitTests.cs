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
			.CustomInstantiator(f =>
				new CreateAuthorDto(f.IndexGlobal + f.Internet.UserName(), f.Internet.Email()));
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
		await Assert.ThrowsAsync<Exception>(() => authorRepository.CreateAuthor(new CreateAuthorDto(author.Name!, author.Email!)));
	}
}