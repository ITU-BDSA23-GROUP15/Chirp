namespace Chirp.Core;

public record CreateCheepDto
{
	public string? Text { get; init; }
	public string? Author { get; init; }

	public CreateCheepDto(string Text, string Author)
	{
		this.Text = Text;
		this.Author = Author;
	}

	public CreateCheepDto() // parameterless constructor is created for the purpose of the unit tests
	{
	}
}