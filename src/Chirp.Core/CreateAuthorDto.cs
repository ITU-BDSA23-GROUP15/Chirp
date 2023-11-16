namespace Chirp.Core;

public record CreateAuthorDto
{
	public string? Name { get; init; }
	public string? Email { get; init; }

	public CreateAuthorDto(string Name, string Email)
	{
		this.Name = Name;
		this.Email = Email;
	}

	public CreateAuthorDto() // parameterless constructor is created for the purpose of the unit tests
	{
	}

}
