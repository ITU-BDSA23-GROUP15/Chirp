namespace Chirp.CheepService;
public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps();
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    DBFacade db = new DBFacade();

    public List<CheepViewModel> GetCheeps()
    {
        List<CheepViewModel> list = new();

        foreach (var cheep in db.GetCheeps()) {
            list.Add(new CheepViewModel(cheep.Author, cheep.Message, UnixTimeStampToDateTimeString(Double.Parse(cheep.Timestamp))));
        }
        return list;
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        // filter by the provided author name
        return GetCheeps().Where(x => x.Author == author).ToList();
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}