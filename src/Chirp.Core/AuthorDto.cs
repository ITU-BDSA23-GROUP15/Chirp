namespace Chirp.Core
{
    public record AuthorDto(Guid AuthorId, string Name, string Email);
    public record CreateAuthorDto(string Name, string Email);

}