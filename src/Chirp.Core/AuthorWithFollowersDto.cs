namespace Chirp.Core
{
    public record AuthorWithFollowersDto(Guid AuthorId, string Name, string Email, List<AuthorDto> Followers, List<AuthorDto> Following);
}