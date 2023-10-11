public interface ICheepRepository : IRepository<Cheep> {
    IEnumerable<Cheep> GetCheeps(int pageRange);
    IEnumerable<Cheep> GetCheepsFromAuthor(string author, int pageRange);
}