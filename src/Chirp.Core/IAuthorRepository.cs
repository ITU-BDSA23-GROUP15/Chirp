namespace Chirp.Core;

public interface IAuthorRepository
{
    Task<AuthorDto> GetAuthorByEmail(string email);
    Task<AuthorDto> GetAuthorByName(string authorName);
    Task<bool> AuthorExists(string authorName);
    Task CreateAuthor(CreateAuthorDto author);
    Task FollowAuthor(string authorName, string authorToFollowName);
    Task UnfollowAuthor(string authorName, string authorToUnfollowName);
    IEnumerable<string> GetAuthorFollowing(string authorName);
    IEnumerable<string> GetAuthorFollowers(string authorName);
}