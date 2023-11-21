namespace Chirp.Core
{
    public record AuthorDto(Guid AuthorId, string Name, string Email, List<CheepDto> Cheeps);

    public record CreateAuthorDto(string Name, string Email);
}