using System.Data.Common;
using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace Application.Testing;

public class InMemoryUnitTests : IDisposable
{
    private readonly DbConnection _connection;
    private readonly DbContextOptions<ChirpContext> _contextOptions;

    public InMemoryUnitTests() {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        _contextOptions = new DbContextOptionsBuilder<ChirpContext>()
            .UseSqlite(_connection).Options;

        using (var context = new ChirpContext(_contextOptions)) 
        {
            context.Database.EnsureCreated();
            context.Cheeps.AddRange(
				new Cheep { Author = new Author { Name = "Mads", Email = "macj@test.com", Cheeps = new List<Cheep>() }, Text = "Mads Chirp 1" },
                new Cheep { Author = new Author { Name = "Rasmus", Email = "raln@test.com", Cheeps = new List<Cheep>() }, Text = "Rasmus Chirp 1 " },
                new Cheep { Author = new Author { Name = "Jacob", Email = "jacp@test.com", Cheeps = new List<Cheep>() }, Text = "Jacob Chirp 1" },
                new Cheep { Author = new Author { Name = "Daniel", Email = "dmil@test.com", Cheeps = new List<Cheep>() }, Text = "Daniel Chirp 1" },
                new Cheep { Author = new Author { Name = "Frederik", Email = "frlr@test.com", Cheeps = new List<Cheep>() }, Text = "Frederik Chirp 1" });
            context.SaveChanges();
        }
        using (var context = new ChirpContext(_contextOptions)) 
        {
            var cheepRepository = new CheepRepository(context);
            var authorRepository = new AuthorRepository(context);
			// cheepRepository.GetCheeps(1, 2).Wait();

            Assert.Single(authorRepository.GetAuthorByName("Mads").Result.Cheeps);
        }
    }
	public void Dispose()
	{
		throw new NotImplementedException();
	}
}