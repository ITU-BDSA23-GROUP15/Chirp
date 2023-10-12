using Chirp.Razor.Repository;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public Task<List<CheepViewModel>> GetCheeps(int pageRange);
    public Task<List<CheepViewModel>> GetCheepsFromAuthor(string author, int pageRange);
}

public class CheepService : ICheepService
{
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

    public async Task<List<CheepViewModel>> GetCheepsFromAuthor(string author, int pageIndex)
    {
        List<CheepViewModel> list = new();
        var cheeps = await _cheepRepository.GetCheepsFromAuthor(author, pageIndex, 32);

        foreach (var cheep in cheeps)
        {
            list.Add(new CheepViewModel(cheep.Author.Name, cheep.Text, cheep.TimeStamp.ToString()));
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