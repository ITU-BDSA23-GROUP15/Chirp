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
			.RuleFor(x => x.Text, f => f.Lorem.Sentence().ClampLength(1, 160));
	}

	[Fact]
	public async Task CreateCheepSavesToDB()
	{
		// Arrange
		DbContext.RemoveRange(DbContext.Cheeps); //Removes all Cheeps made in former tests
		DbContext.RemoveRange(DbContext.Authors); //Removes all Authors made in former tests
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
		DbContext.RemoveRange(DbContext.Cheeps); //Removes all Cheeps made in former tests
		DbContext.RemoveRange(DbContext.Authors); //Removes all Authors made in former tests
		var fakeAuthor = authorGenerator.Generate();
		DbContext.Authors.Add(fakeAuthor);
		await DbContext.SaveChangesAsync();
		var createCheepDto = new CreateCheepDto(cheepGenerator.Generate().Text!, fakeAuthor.Name!);

		// Act
		await cheepRepository.CreateCheep(createCheepDto);

		// Assert
		Assert.Equal(createCheepDto.Text, DbContext.Cheeps.First().Text);
		Assert.Equal(createCheepDto.AuthorName, DbContext.Cheeps.First().Author.Name);
	}

	[Fact]
	public async Task CreateCheepSavesCorrectDate()
	{
		// Arrange
		DbContext.RemoveRange(DbContext.Cheeps); //Removes all Cheeps made in former tests
		DbContext.RemoveRange(DbContext.Authors); //Removes all Authors made in former tests
		var fakeAuthor = authorGenerator.Generate();
		DbContext.Authors.Add(fakeAuthor);
		await DbContext.SaveChangesAsync();
		var createCheepDto = new CreateCheepDto(cheepGenerator.Generate().Text!, fakeAuthor.Name!);

		// Act
		await cheepRepository.CreateCheep(createCheepDto);

		// Assert
		Assert.Equal(DateTime.Now.Date, DbContext.Cheeps.First().TimeStamp.Date);
	}

	[Fact]
	public async Task CreateCheepThrowsExceptionWhenTextIsTooLong()
	{
		// Arrange
		DbContext.RemoveRange(DbContext.Cheeps); //Removes all Cheeps made in former tests
		DbContext.RemoveRange(DbContext.Authors); //Removes all Authors made in former tests
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
		DbContext.RemoveRange(DbContext.Cheeps); //Removes all Cheeps made in former tests
		DbContext.RemoveRange(DbContext.Authors); //Removes all Authors made in former tests
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
		DbContext.RemoveRange(DbContext.Cheeps); //Removes all Cheeps made in former tests
		DbContext.RemoveRange(DbContext.Authors); //Removes all Authors made in former tests

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
		DbContext.RemoveRange(DbContext.Cheeps); //Removes all Cheeps made in former tests
		DbContext.RemoveRange(DbContext.Authors); //Removes all Authors made in former tests
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
	public async Task GetCheepsReturnsOnly32()
	{
		// Arrange
		DbContext.RemoveRange(DbContext.Cheeps); //Removes all Cheeps made in former tests
		DbContext.RemoveRange(DbContext.Authors); //Removes all Authors made in former tests

		var fakeAuthor = authorGenerator.Generate();
		var fakeCheeps = cheepGenerator
			.RuleFor(a => a.Author, fakeAuthor)
			.RuleFor(a => a.AuthorId, fakeAuthor.AuthorId)
			.Generate(50);
		DbContext.Authors.Add(fakeAuthor);
		DbContext.Cheeps.AddRange(fakeCheeps);
		await DbContext.SaveChangesAsync();


		// Act
		var cheeps = await cheepRepository.GetCheeps(1, 32); // first page, 32 cheeps taken

		// Assert
		Assert.Equal(10, cheeps.Count());
	}

	// 	[Fact]
	// 	public async Task GetCheepsReturn1CheepFromAuthor()
	// 	{
	// 		// Arrange
	// 		DbContext.RemoveRange(DbContext.Cheeps); //Removes all Cheeps made in former tests
	// 		DbContext.RemoveRange(DbContext.Authors); //Removes all Authors made in former tests

	// 		var specificAuthor = authorGenerator.Generate();

	// 		await authorRepository.CreateAuthor(specificAuthor);
	// 		await cheepRepository.CreateCheep(new CreateCheepDto(cheepGenerator.Generate().Text!, specificAuthor.Name!));

	// 		foreach (var fakeCheep in cheepGenerator.Generate(10))
	// 		{
	// 			var fakeAuthor = authorGenerator.Generate();
	// 			await authorRepository.CreateAuthor(fakeAuthor);
	// 			await cheepRepository.CreateCheep(new CreateCheepDto(fakeCheep.Text!, fakeAuthor.Name!));
	// 		}

	// 		// Act
	// 		var cheeps = await cheepRepository.GetCheepsFromAuthor(specificAuthor.Name!, 1, 32); // first page, 32 cheeps taken

	// 		// Assert
	// 		Assert.Single(cheeps);
	// 	}
}