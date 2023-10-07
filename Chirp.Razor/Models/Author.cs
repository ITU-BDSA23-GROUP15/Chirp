
public class Author 
{
    public required string Name { get; set; }
    public string Email { get; set; }
    public list<Cheep> Cheeps { get; set; }
}