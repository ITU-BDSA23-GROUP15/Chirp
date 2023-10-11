
using System.Linq.Expressions;
using Chirp.Razor.Repository;

public class AuthorRepository : IAuthorRepository
{
    public void Add(Author entity)
    {
        throw new NotImplementedException();
    }

    public Task<Author> Get(Expression<Func<Author, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Author>> GetAll()
    {
        throw new NotImplementedException();
    }
}