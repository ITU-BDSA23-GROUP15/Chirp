namespace Chirp.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Chirp.Core;

public class AuthorRepository : IAuthorRepository
{
    private readonly ChirpContext _context;

    public AuthorRepository(ChirpContext context)
    {
        _context = context;
    }

    public async Task<AuthorDto> GetAuthorByName(string name)
    {
        var author = await _context.Authors
            .Include(a => a.Cheeps)
            .Where(a => a.Name == name)
            .Select(a => new AuthorDto
                (a.AuthorId,
                a.Name,
                a.Email,
                a.Cheeps.Select(c => new CheepDto(c.Text, c.Author.Name, c.TimeStamp)).ToList())
            )
            .FirstOrDefaultAsync();

        if (author == null)
        {
            throw new Exception("Author doesn't exist");
        }

        return author;
    }

    public async Task CreateAuthor(CreateAuthorDto author)
    {
        var newAuthor = new Author
        {
            AuthorId = Guid.NewGuid(),
            Name = author.Name,
            Email = author.Email,
            Cheeps = new List<Cheep>(),
            Followers = new List<Follower>()
        };

        await _context.Authors.AddAsync(newAuthor);
        await _context.SaveChangesAsync();
    }

    public async Task<AuthorDto> GetAuthorByEmail(string email)
    {
        var author = await _context.Authors
            .Include(a => a.Cheeps)
            .Where(a => a.Email == email)
            .Select(a => new AuthorDto(
                a.AuthorId,
                a.Name,
                a.Email,
                a.Cheeps.Select(c => new CheepDto(c.Text, c.Author.Name, c.TimeStamp)).ToList())
            )
            .FirstOrDefaultAsync();

        if (author == null)
        {
            throw new Exception("Author doesn't exist");
        }

        return author;
    }

    public async Task<bool> AuthorExists(string name){
        return await _context.Authors.AnyAsync(a => a.Name == name);
    }
}
