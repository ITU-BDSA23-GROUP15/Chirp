using Microsoft.EntityFrameworkCore;

public class CheepRepository : ICheepRepository {
    private readonly ChirpContext _context;

    CheepRepository(ChirpContext context){
        _context = context;
    }

    public List<Cheep> GetCheepsFromAuthor(string author, int pageRange)
    {
        throw new NotImplementedException();
    }

    Cheep Add(Cheep cheep){
        _context.Add(cheep);
        _context.SaveChanges();
        return cheep;
    }

    Cheep IRepository<Cheep>.Add(Cheep entity)
    {
        throw new NotImplementedException();
    }

    Cheep? Get(int id){
        return _context.Cheeps.Find(id);
    }

    Cheep IRepository<Cheep>.Get(int id)
    {
        throw new NotImplementedException();
    }

    IEnumerable<Cheep> GetAll(){
        return _context.Cheeps.ToList();
    }

    IEnumerable<Cheep> IRepository<Cheep>.GetAll()
    {
        throw new NotImplementedException();
    }

    IEnumerable<Cheep> ICheepRepository.GetCheeps(int pageRange)
    {
        throw new NotImplementedException();
    }

    IEnumerable<Cheep> ICheepRepository.GetCheepsFromAuthor(string author, int pageRange)
    {
        throw new NotImplementedException();
    }
}