namespace Chirp.Core;

public interface IAuthorRepository
{
    Task<AuthorDto> GetAuthorByEmail(string email);
    Task<AuthorDto> GetAuthorByName(string authorName);
    IEnumerable<string> GetAuthorFollowers(string authorName);
    IEnumerable<string> GetAuthorFollowing(string authorName);
    Task<bool> AuthorExists(string authorName);
    Task CreateAuthor(CreateAuthorDto author);
    Task DeleteAuthor(string authorName);
    Task FollowAuthor(string authorName, string authorToFollowName);
    Task UnfollowAuthor(string authorName, string authorToUnfollowName);
}