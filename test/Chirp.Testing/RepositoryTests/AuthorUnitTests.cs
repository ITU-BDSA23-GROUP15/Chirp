using Chirp.Core;
using Microsoft.EntityFrameworkCore;

namespace Application.Testing;

public class AuthorUnitTests : BaseIntegrationTest
{
	private readonly AuthorRepository authorRepository;
	private readonly Faker<Author> authorGenerator;

	public AuthorUnitTests(IntegrationTestWebAppFactory factory) : base(factory)
	{
		authorRepository = new AuthorRepository(DbContext);

		authorGenerator = new Faker<Author>()
			.RuleFor(a => a.Name, f => f.Internet.UserName())
			.RuleFor(a => a.Email, f => f.Internet.Email());
	}

	[Fact]
	public async Task CreateAuthorSavesToDB()
	{
		// Arrange
		DbContext.RemoveRange(DbContext.Authors);
		await DbContext.SaveChangesAsync();
		var fakeAuthor = authorGenerator.Generate();
		var createAuthorDto = new CreateAuthorDto(fakeAuthor.Name, fakeAuthor.Email);

		// Act
		await authorRepository.CreateAuthor(createAuthorDto);

		// Assert
		Assert.Single(DbContext.Authors);
	}

	[Fact]
	public async Task CreateAuthorSavesCorrectInfo()
	{
		// Arrange
		DbContext.RemoveRange(DbContext.Authors);
		await DbContext.SaveChangesAsync();
		var fakeAuthor = authorGenerator.Generate();
		var createAuthorDto = new CreateAuthorDto(fakeAuthor.Name, fakeAuthor.Email);

		// Act
		await authorRepository.CreateAuthor(createAuthorDto);

		// Assert
		Assert.Equal(createAuthorDto.Name, DbContext.Authors.First().Name);
		Assert.Equal(createAuthorDto.Email, DbContext.Authors.First().Email);
	}

	[Fact]
	public async Task CreatingExistingNameThrowsException()
	{
		// Arrange
		DbContext.RemoveRange(DbContext.Authors);
		var author = authorGenerator.Generate();
		DbContext.Add(author);
		await DbContext.SaveChangesAsync();

		// Act
		// Assert
		await Assert.ThrowsAsync<Exception>(() => authorRepository.CreateAuthor(new CreateAuthorDto(author.Name, author.Email!)));
	}

	[Fact]
	public async Task DeleteAuthorRemovesAuthorFromDB()
	{
		// Arrange
		DbContext.RemoveRange(DbContext.Authors);
		var author = authorGenerator.Generate();
		DbContext.Add(author);
		await DbContext.SaveChangesAsync();

		// Act
		await authorRepository.DeleteAuthor(author.Name);

		// Assert
		Assert.Empty(DbContext.Authors);
	}

	[Fact]
	public async Task AuthorExistsReturnsTrueIfAuthorExists()
	{
		// Arrange
		DbContext.RemoveRange(DbContext.Authors);
		var author = authorGenerator.Generate();
		DbContext.Add(author);
		await DbContext.SaveChangesAsync();

		// Act
		var result = await authorRepository.AuthorExists(author.Name);

		// Assert
		Assert.True(result);
	}

	[Fact]
	public async Task AuthorExistsReturnsFalseIfAuthorDoesntExist()
	{
		// Arrange
		DbContext.RemoveRange(DbContext.Authors);
		var author = authorGenerator.Generate();
		DbContext.Add(author);
		await DbContext.SaveChangesAsync();

		// Act
		var result = await authorRepository.AuthorExists("not an author");

		// Assert
		Assert.False(result);
	}

	[Fact]
	public async Task FollowAuthorAddsAuthorsToFollowingAndFollowers()
	{
		// Arrange
		DbContext.RemoveRange(DbContext.Authors);
		var author = authorGenerator.Generate();
		var authorToFollow = authorGenerator.Generate();
		DbContext.Add(author);
		DbContext.Add(authorToFollow);
		await DbContext.SaveChangesAsync();

		// Act
		await authorRepository.FollowAuthor(author.Name, authorToFollow.Name);

		// Assert
		Assert.Single(DbContext.Authors
			.Include(a => a.Following)
			.First(a => a.Name == author.Name)
			.Following);
		Assert.Single(DbContext.Authors
			.Include(a => a.Followers)
			.First(a => a.Name == authorToFollow.Name)
			.Followers);
	}

	[Fact]
	public async Task FollowAuthorThrowsExceptionIfAuthorToFollowDoesntExist()
	{
		// Arrange
		DbContext.RemoveRange(DbContext.Authors);
		var author = authorGenerator.Generate();
		var authorToFollow = authorGenerator.Generate();
		DbContext.Add(author);
		await DbContext.SaveChangesAsync();

		// Act
		// Assert
		await Assert.ThrowsAsync<Exception>(() => authorRepository.FollowAuthor(author.Name, authorToFollow.Name));
	}

	[Fact]
	public async Task FollowAuthorThrowsExceptionIfAuthorDoesntExist()
	{
		// Arrange
		DbContext.RemoveRange(DbContext.Authors);
		var author = authorGenerator.Generate();
		var authorToFollow = authorGenerator.Generate();
		DbContext.Add(authorToFollow);
		await DbContext.SaveChangesAsync();

		// Act
		// Assert
		await Assert.ThrowsAsync<Exception>(() => authorRepository.FollowAuthor(author.Name, authorToFollow.Name));
	}

	[Fact]
	public async Task FollowAuthorDoesntAddDuplicateFollowers()
	{
		// Arrange
		DbContext.RemoveRange(DbContext.Authors);
		var author = authorGenerator.Generate();
		var authorToFollow = authorGenerator.Generate();
		author.Following.Add(authorToFollow);
		authorToFollow.Followers.Add(author);
		DbContext.Add(author);
		DbContext.Add(authorToFollow);
		await DbContext.SaveChangesAsync();

		// Act
		// Assert
		await Assert.ThrowsAsync<Exception>(() => authorRepository.FollowAuthor(author.Name, authorToFollow.Name));
	}

	[Fact]
	public async Task UnfollowAuthorRemovesAuthorsFromFollowingAndFollowers()
	{
		// Arrange
		DbContext.RemoveRange(DbContext.Authors);
		var author = authorGenerator.Generate();
		var authorToUnfollow = authorGenerator.Generate();
		author.Following.Add(authorToUnfollow);
		authorToUnfollow.Followers.Add(author);
		DbContext.Add(author);
		DbContext.Add(authorToUnfollow);
		await DbContext.SaveChangesAsync();

		// Act
		await authorRepository.UnfollowAuthor(author.Name, authorToUnfollow.Name);

		// Assert
		Assert.Empty(DbContext.Authors
			.Include(a => a.Following)
			.First(a => a.Name == author.Name)
			.Following);
		Assert.Empty(DbContext.Authors
			.Include(a => a.Followers)
			.First(a => a.Name == authorToUnfollow.Name)
			.Followers);
	}

	[Fact]
	public async Task GetAuthorFollowingReturnsCorrectAuthors()
	{
		// Arrange
		DbContext.RemoveRange(DbContext.Authors);
		var author = authorGenerator.Generate();
		var authorToFollow = authorGenerator.Generate();
		author.Following.Add(authorToFollow);
		authorToFollow.Followers.Add(author);
		DbContext.Add(author);
		DbContext.Add(authorToFollow);
		await DbContext.SaveChangesAsync();

		// Act
		var result = authorRepository.GetAuthorFollowing(author.Name);

		// Assert
		Assert.Single(result);
		Assert.Equal(authorToFollow.Name, result.First());
	}

	[Fact]
	public async Task GetAuthorFollowersReturnsCorrectAuthors()
	{
		// Arrange
		DbContext.RemoveRange(DbContext.Authors);
		var author = authorGenerator.Generate();
		var authorToFollow = authorGenerator.Generate();
		author.Following.Add(authorToFollow);
		authorToFollow.Followers.Add(author);
		DbContext.Add(author);
		DbContext.Add(authorToFollow);
		await DbContext.SaveChangesAsync();

		// Act
		var result = authorRepository.GetAuthorFollowers(authorToFollow.Name);

		// Assert
		Assert.Single(result);
		Assert.Equal(author.Name, result.First());
	}
}