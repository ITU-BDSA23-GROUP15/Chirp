using Bogus;
using Chirp.Core;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Testing;

public class CheepUnitTests
{
	Faker<Author> authorGenerator;
	public CheepUnitTests()
	{
		authorGenerator = new Faker<Author>()
			.RuleFor(u => u.AuthorId, (f, u) => f.Random.Guid())
			.RuleFor(u => u.Name, (f, u) => f.Name.LastName())
			.RuleFor(u => u.Email, (f, u) => f.Internet.Email());
	}

	[Fact]
	public void CreateCheepTest()
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

}