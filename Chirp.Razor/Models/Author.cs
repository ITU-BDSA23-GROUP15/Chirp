public class Author 
{
    public required string Name { get; set; }
    public string Email { get; set; }
    public List<Cheep> Cheeps { get; set; }
}