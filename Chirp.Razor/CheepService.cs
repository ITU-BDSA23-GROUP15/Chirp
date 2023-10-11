public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public Task<List<CheepViewModel>> GetCheeps(int pageRange);
    public List<CheepViewModel> GetCheepsFromAuthor(string author, int pageRange);
}

public class CheepService : ICheepService
{
    DBFacade db = new();
    private readonly ICheepRepository _cheepRepository;
    private readonly IAuthorRepository _authorRepository;

    public CheepService(ICheepRepository cheepRepository, IAuthorRepository authorRepository)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
    }

    public async Task<List<CheepViewModel>> GetCheeps(int pageIndex)
    {
        List<CheepViewModel> list = new();
        var cheeps = await _cheepRepository.GetCheeps(pageIndex,32);

        foreach (var cheep in cheeps)
        {
            list.Add(new CheepViewModel(cheep.Author.Name, cheep.Text, cheep.TimeStamp.ToString()));
        }
        return list;
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author, int pageRange)
    {
        List<CheepViewModel> list = new();
        var query = $"SELECT * FROM message m JOIN user u ON u.user_id = m.author_id WHERE u.username = \"{author}\" ORDER by m.pub_date desc LIMIT 32 OFFSET {pageRange}";

        foreach (var cheep in db.GetCheeps(query))
        {
            list.Add(new CheepViewModel(cheep.Author, cheep.Message, UnixTimeStampToDateTimeString(Double.Parse(cheep.Timestamp))));
        }
        return list;
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
}