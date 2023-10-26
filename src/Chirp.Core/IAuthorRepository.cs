namespace Chirp.Core;

public interface IAuthorRepository
{
    Task<AuthorDto> GetAuthorByEmail(string email);
    Task<AuthorDto> GetAuthorByName(string name);
    void CreateAuthor(string name, string email);
}