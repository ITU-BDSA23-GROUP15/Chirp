namespace Chirp.Core
{
    public record AuthorDto(Guid Id, string Name, string Email, List<CheepDto> Cheeps);
}