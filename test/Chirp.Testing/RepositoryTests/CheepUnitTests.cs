using Chirp.Core;
using Bogus.Extensions;

namespace Application.Testing;

public class CheepUnitTests : BaseIntegrationTest
{
	private readonly CheepRepository cheepRepository;
	private readonly AuthorRepository authorRepository;
	private readonly Faker<Author> authorGenerator;
	private readonly Faker<Cheep> cheepGenerator;
	public CheepUnitTests(IntegrationTestWebAppFactory factory) : base(factory)
	{
		cheepRepository = new CheepRepository(DbContext, new CreateCheepValidator());
		authorRepository = new AuthorRepository(DbContext);

		authorGenerator = new Faker<Author>()
			.RuleFor(a => a.Name, f => f.Internet.UserName())
			.RuleFor(a => a.Email, f => f.Internet.Email());

		cheepGenerator = new Faker<Cheep>()
			.RuleFor(x => x.Text, f => f.Lorem.Sentence().ClampLength(1, 160))
			.RuleFor(x => x.TimeStamp, f => f.Date.Past());
	}

	private void ClearDB() {
		DbContext.RemoveRange(DbContext.Cheeps); //Removes all Cheeps made in former tests
		DbContext.RemoveRange(DbContext.Authors); //Removes all Authors made in former tests
	}

	[Fact]
	public async Task CreateCheepSavesToDB()
	{
		// Arrange
		ClearDB();
		var fakeAuthor = authorGenerator.Generate();
		DbContext.Authors.Add(fakeAuthor);
		await DbContext.SaveChangesAsync();
		var createCheepDto = new CreateCheepDto(cheepGenerator.Generate().Text!, fakeAuthor.Name!);

		// Act
		await cheepRepository.CreateCheep(createCheepDto);

		// Assert
		Assert.Single(DbContext.Cheeps);
	}

	[Fact]
	public async Task CreateCheepSavesCorrectInfo()
	{
		// Arrange
		ClearDB();
		var fakeAuthor = authorGenerator.Generate();
		DbContext.Authors.Add(fakeAuthor);
		await DbContext.SaveChangesAsync();
		var createCheepDto = new CreateCheepDto(cheepGenerator.Generate().Text!, fakeAuthor.Name!);

		// Act
		await cheepRepository.CreateCheep(createCheepDto);

		// Assert
		Assert.Equal(createCheepDto.Text, DbContext.Cheeps.First().Text);
		Assert.Equal(createCheepDto.AuthorName, DbContext.Cheeps.First().Author.Name);
		Assert.Equal(DateTime.Now.Date, DbContext.Cheeps.First().TimeStamp.Date);
	}

	[Fact]
	public async Task CreateCheepThrowsExceptionWhenTextIsTooLong()
	{
		// Arrange
		ClearDB();
		var fakeAuthor = authorGenerator.Generate();
		DbContext.Authors.Add(fakeAuthor);
		await DbContext.SaveChangesAsync();

		var createCheepDto = new CreateCheepDto(cheepGenerator.Generate().Text!.PadRight(161), fakeAuthor.Name);

		// Act
		// Assert
		await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => cheepRepository.CreateCheep(createCheepDto));
	}

	[Fact]
	public async Task CreateCheepThrowsExceptionWhenTextIsEmpty()
	{
		// Arrange
		ClearDB();
		var fakeAuthor = authorGenerator.Generate();
		DbContext.Authors.Add(fakeAuthor);
		await DbContext.SaveChangesAsync();

		var createCheepDto = new CreateCheepDto("", fakeAuthor.Name);

		// Act
		// Assert
		await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => cheepRepository.CreateCheep(createCheepDto));
	}

	[Fact]
	public async Task DeleteCheepRemovesCheepFromDB()
	{
		// Arrange
		ClearDB();
		var fakeAuthor = authorGenerator.Generate();
		var fakeCheep = cheepGenerator
			.RuleFor(a => a.Author, fakeAuthor)
			.RuleFor(a => a.AuthorId, fakeAuthor.AuthorId)
			.Generate();
		DbContext.Authors.Add(fakeAuthor);
		DbContext.Cheeps.Add(fakeCheep);
		await DbContext.SaveChangesAsync();

		// Act
		await cheepRepository.DeleteCheep(DbContext.Cheeps.First().CheepId);

		// Assert
		Assert.Empty(DbContext.Cheeps);
	}


	[Fact]
	public async Task GetCheepsReturnsAny()
	{
		// Arrange
		ClearDB();
		var fakeAuthor = authorGenerator.Generate();
		var fakeCheep = cheepGenerator
			.RuleFor(a => a.Author, fakeAuthor)
			.RuleFor(a => a.AuthorId, fakeAuthor.AuthorId)
			.Generate();
		DbContext.Authors.Add(fakeAuthor);
		DbContext.Cheeps.Add(fakeCheep);
		await DbContext.SaveChangesAsync();

		// Act
		var result = await cheepRepository.GetCheeps(1, 32); // first page, 32 cheeps taken

		// Assert
		Assert.NotNull(result);
	}

	[Fact]
	public async Task GetCheepsReturnsCorrectInfo()
	{
		// Arrange
		ClearDB();
		var fakeAuthor = authorGenerator.Generate();
		var fakeCheep = cheepGenerator
			.RuleFor(a => a.Author, fakeAuthor)
			.RuleFor(a => a.AuthorId, fakeAuthor.AuthorId)
			.Generate();
		DbContext.Authors.Add(fakeAuthor);
		DbContext.Cheeps.Add(fakeCheep);
		await DbContext.SaveChangesAsync();

		// Act
		var result = await cheepRepository.GetCheeps(1, 32); // first page, 32 cheeps taken

		// Assert
		Assert.Equal(fakeCheep.Text, result.First().Text);
		Assert.Equal(fakeCheep.Author.Name, result.First().AuthorName);
		Assert.Equal(fakeCheep.TimeStamp.Date, result.First().TimeStamp.Date);
	}

	[Fact]
	public async Task GetCheepsReturnsOnly32()
	{
		// Arrange
		ClearDB();
		var fakeAuthor = authorGenerator.Generate();
		var fakeCheeps = cheepGenerator
			.RuleFor(a => a.Author, fakeAuthor)
			.RuleFor(a => a.AuthorId, fakeAuthor.AuthorId)
			.Generate(35);
		DbContext.Authors.Add(fakeAuthor);
		DbContext.Cheeps.AddRange(fakeCheeps);
		await DbContext.SaveChangesAsync();

		// Act
		var result = await cheepRepository.GetCheeps(1, 32); // first page, 32 cheeps taken

		// Assert
		Assert.Equal(32, result.Count());
	}

	[Fact]
	public async Task GetCheepsReturnsSecondPage()
	{
		// Arrange
		ClearDB();
		var fakeAuthor = authorGenerator.Generate();
		var fakeCheeps = cheepGenerator
			.RuleFor(a => a.Author, fakeAuthor)
			.RuleFor(a => a.AuthorId, fakeAuthor.AuthorId)
			.Generate(35);
		DbContext.Authors.Add(fakeAuthor);
		DbContext.Cheeps.AddRange(fakeCheeps);
		await DbContext.SaveChangesAsync();

		// Act
		var result = await cheepRepository.GetCheeps(2, 32); // Second page, 32 cheeps taken

		// Assert
		Assert.Equal(3, result.Count());
	}

	[Fact]
	public async Task GetCheepsFromAuthorReturnsAny()
	{
		// Arrange
		ClearDB();
		var fakeAuthor = authorGenerator.Generate();
		var fakeCheep = cheepGenerator
			.RuleFor(a => a.Author, fakeAuthor)
			.RuleFor(a => a.AuthorId, fakeAuthor.AuthorId)
			.Generate();
		DbContext.Authors.Add(fakeAuthor);
		DbContext.Cheeps.Add(fakeCheep);
		await DbContext.SaveChangesAsync();

		// Act
		var result = await cheepRepository.GetCheepsFromAuthor(fakeAuthor.Name, 1, 32); // first page, 32 cheeps taken

		// Assert
		Assert.NotNull(result);
	}

	[Fact]
	public async Task GetCheepsFromAuthorReturnsCorrectInfo()
	{
		// Arrange
		ClearDB();
		var fakeAuthor = authorGenerator.Generate();
		var fakeCheep = cheepGenerator
			.RuleFor(a => a.Author, fakeAuthor)
			.RuleFor(a => a.AuthorId, fakeAuthor.AuthorId)
			.Generate();
		DbContext.Authors.Add(fakeAuthor);
		DbContext.Cheeps.Add(fakeCheep);
		await DbContext.SaveChangesAsync();

		// Act
		var result = await cheepRepository.GetCheepsFromAuthor(fakeAuthor.Name, 1, 32); // first page, 32 cheeps taken

		// Assert
		Assert.Equal(fakeCheep.Text, result.First().Text);
		Assert.Equal(fakeCheep.Author.Name, result.First().AuthorName);
		Assert.Equal(fakeCheep.TimeStamp.Date, result.First().TimeStamp.Date);
	}

	[Fact]
	public async Task GetCheepsFromAuthorReturnsOnly32()
	{
		// Arrange
		ClearDB();
		var fakeAuthor = authorGenerator.Generate();
		var fakeCheeps = cheepGenerator
			.RuleFor(a => a.Author, fakeAuthor)
			.RuleFor(a => a.AuthorId, fakeAuthor.AuthorId)
			.Generate(35);
		DbContext.Authors.Add(fakeAuthor);
		DbContext.Cheeps.AddRange(fakeCheeps);
		await DbContext.SaveChangesAsync();

		// Act
		var result = await cheepRepository.GetCheepsFromAuthor(fakeAuthor.Name, 1, 32); // first page, 32 cheeps taken

		// Assert
		Assert.Equal(32, result.Count());
	}

	[Fact]
	public async Task GetCheepsFromAuthorReturnsSecondPage()
	{
		// Arrange
		ClearDB();
		var fakeAuthor = authorGenerator.Generate();
		var fakeCheeps = cheepGenerator
			.RuleFor(a => a.Author, fakeAuthor)
			.RuleFor(a => a.AuthorId, fakeAuthor.AuthorId)
			.Generate(35);
		DbContext.Authors.Add(fakeAuthor);
		DbContext.Cheeps.AddRange(fakeCheeps);
		await DbContext.SaveChangesAsync();

		// Act
		var result = await cheepRepository.GetCheeps(2, 32); // Second page, 32 cheeps taken

		// Assert
		Assert.Equal(3, result.Count());
	}

	[Fact]
	public async Task GetCheepsFromAuthorOnlyReturnsCheepsFromAuthor()
	{
		// Arrange
		ClearDB();
		var fakeAuthor1 = authorGenerator.Generate();
		var fakeAuthor2 = authorGenerator.Generate();
		var fakeCheep1 = cheepGenerator
			.RuleFor(a => a.Author, fakeAuthor1)
			.RuleFor(a => a.AuthorId, fakeAuthor1.AuthorId)
			.Generate();
		var fakeCheep2 = cheepGenerator
			.RuleFor(a => a.Author, fakeAuthor2)
			.RuleFor(a => a.AuthorId, fakeAuthor2.AuthorId)
			.Generate();
		DbContext.Authors.Add(fakeAuthor1);
		DbContext.Authors.Add(fakeAuthor2);
		DbContext.Cheeps.Add(fakeCheep1);
		DbContext.Cheeps.Add(fakeCheep2);
		await DbContext.SaveChangesAsync();

		// Act
		var result = await cheepRepository.GetCheepsFromAuthor(fakeAuthor1.Name, 1, 32); // first page, 32 cheeps taken

		// Assert
		Assert.All(result, cheep => Assert.Equal(fakeAuthor1.Name, cheep.AuthorName));
	}

	[Fact]
	public async Task GetPersonalCheepsOnlyReturnsCheepsFromAuthorAndFollowing()
	{
		// Arrange
		ClearDB();
		var fakeAuthor1 = authorGenerator.Generate();
		var fakeAuthor2 = authorGenerator.Generate();
		var fakeAuthor3 = authorGenerator.Generate();
		fakeAuthor1.Following.Add(fakeAuthor2);
		var fakeCheep1 = cheepGenerator
			.RuleFor(a => a.Author, fakeAuthor1)
			.RuleFor(a => a.AuthorId, fakeAuthor1.AuthorId)
			.Generate();
		var fakeCheep2 = cheepGenerator
			.RuleFor(a => a.Author, fakeAuthor2)
			.RuleFor(a => a.AuthorId, fakeAuthor2.AuthorId)
			.Generate();
		var fakeCheep3 = cheepGenerator
			.RuleFor(a => a.Author, fakeAuthor3)
			.RuleFor(a => a.AuthorId, fakeAuthor3.AuthorId)
			.Generate();
		DbContext.Authors.Add(fakeAuthor1);
		DbContext.Authors.Add(fakeAuthor2);
		DbContext.Authors.Add(fakeAuthor3);
		DbContext.Cheeps.Add(fakeCheep1);
		DbContext.Cheeps.Add(fakeCheep2);
		DbContext.Cheeps.Add(fakeCheep3);
		await DbContext.SaveChangesAsync();

		// Act
		var result = await cheepRepository.GetPersonalCheeps(fakeAuthor1.Name, 1, 32); // first page, 32 cheeps taken

		// Assert
		Assert.All(result, cheep => Assert.True(cheep.AuthorName == fakeAuthor1.Name || cheep.AuthorName == fakeAuthor2.Name));
	}
}