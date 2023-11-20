namespace Chirp.Core
{
    public record AuthorWithCheepsDto(Guid AuthorId, string Name, string Email, List<CheepDto> Cheeps);
}