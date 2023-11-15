namespace Chirp.Core;

public interface IAuthorRepository
{
    Task<AuthorDto> GetAuthorByEmail(string email);
    Task<AuthorDto> GetAuthorByName(string name);
    Task<bool> AuthorExists(string name);
    Task CreateAuthor(CreateAuthorDto author);
}