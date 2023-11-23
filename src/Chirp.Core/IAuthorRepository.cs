namespace Chirp.Core;

public interface IAuthorRepository
{
    Task<AuthorDto> GetAuthorByEmail(string email);
    Task<AuthorDto> GetAuthorByName(string authorName);
    Task<bool> AuthorExists(string authorName);
    Task CreateAuthor(CreateAuthorDto author);
    Task FollowAuthor(Guid authorId, Guid authorToFollowId);
    Task UnfollowAuthor(Guid authorId, Guid authorToUnfollowId);
    Task<AuthorWithFollowersDto> GetAuthorWithFollowers(Guid authorId);
    IEnumerable<string> GetAuthorFollowing(string authorName);
}