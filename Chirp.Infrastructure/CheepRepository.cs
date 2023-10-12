namespace Chirp.Infrastructure;

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Chirp.Core;

public class CheepRepository : ICheepRepository
{
    private readonly ChirpContext _context;

    public CheepRepository(ChirpContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CheepDto>> GetCheeps(int pageIndex, int pageRange)
    {
        return await _context.Cheeps
            .Include(c => c.Author)
            .Skip((pageIndex - 1) * pageRange)
            .Take(pageRange)
            .Select(c => new CheepDto(c.Text, c.Author.Name, c.TimeStamp))
            .ToListAsync();
    }

    public async Task<IEnumerable<CheepDto>> GetCheepsFromAuthor(string author, int pageIndex, int pageRange)
    {
        return await _context.Cheeps
            .Include(c => c.Author)
            .Where(c => c.Author.Name == author)
            .Skip((pageIndex - 1) * pageRange)
            .Take(pageRange)
            .Select(c => new CheepDto(c.Text, c.Author.Name, c.TimeStamp))
            .ToListAsync();
    }
}