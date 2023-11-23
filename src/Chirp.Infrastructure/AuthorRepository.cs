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
                (
                    a.AuthorId,
                    a.Name,
                    a.Email
                )
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
            Cheeps = new List<Cheep>()
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
                a.Email
            )
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

    public async Task FollowAuthor(string authorName, string authorToFollowName) {
        var author = await _context.Authors
            .Include(a => a.Following)
            .FirstOrDefaultAsync(a => a.Name == authorName);

        var authorToFollow = await _context.Authors
            .Include(a => a.Followers)
            .FirstOrDefaultAsync(a => a.Name == authorToFollowName);

        if (author == null || authorToFollow == null) {
            throw new Exception("Author doesn't exist");
        }

        author.Following.Add(authorToFollow);
        authorToFollow.Followers.Add(author);

        await _context.SaveChangesAsync();
    }

    public async Task UnfollowAuthor(string authorName, string authorToUnfollowName) {
        var author = await _context.Authors
            .Include(a => a.Following)
            .FirstOrDefaultAsync(a => a.Name == authorName);

        var authorToUnfollow = await _context.Authors
            .Include(a => a.Followers)
            .FirstOrDefaultAsync(a => a.Name == authorToUnfollowName);

        if (author == null || authorToUnfollow == null) {
            throw new Exception("Author doesn't exist");
        }

        author.Following.Remove(authorToUnfollow);
        authorToUnfollow.Followers.Remove(author);

        await _context.SaveChangesAsync();
    }

    public IEnumerable<string> GetAuthorFollowing(string authorName) {
        var author = _context.Authors
            .Where(a => a.Name == authorName)
            .SelectMany(a => a.Following)
            .Select(a => a.Name);

        return author;
    }

    public IEnumerable<string> GetAuthorFollowers(string authorName) {
        var author = _context.Authors
            .Where(a => a.Name == authorName)
            .SelectMany(a => a.Followers)
            .Select(a => a.Name);

        return author;
    }
}
