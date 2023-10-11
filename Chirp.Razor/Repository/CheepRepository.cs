using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

public class CheepRepository : ICheepRepository
{
    private readonly ChirpContext _context;

    public CheepRepository(ChirpContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Cheep>> GetCheeps(int pageIndex, int pageRange)
    {
        return await _context.Cheeps
            .Skip((pageIndex - 1) * pageRange)
            .Take(pageRange)
            .ToListAsync();
    }

    public async Task<IEnumerable<Cheep>> GetCheepsFromAuthor(string author, int pageIndex)
    {
        return await _context.Cheeps
            .Where(c => c.Author.Name == author)
            .Skip((pageIndex - 1) * 32)
            .Take(32)
            .ToListAsync();
    }

    public async Task<Cheep?> Get(Expression<Func<Cheep, bool>> predicate)
    {
        return await _context.Cheeps
            .Where(predicate)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Cheep>> GetAll()
    {
        return await _context.Cheeps.ToListAsync();
    }

    public void Add(Cheep cheep)
    {
        _context.Cheeps.Add(cheep);
        _context.SaveChanges();
    }
}