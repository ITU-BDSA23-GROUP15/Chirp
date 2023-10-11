using Chirp.Razor.Repository;

public interface ICheepRepository : IRepository<Cheep> 
{
    Task<IEnumerable<Cheep>> GetCheeps(int pageIndex, int pageRange);
    Task<IEnumerable<Cheep>> GetCheepsFromAuthor(string author, int pageIndex, int pageRange);
}