using System.Linq.Expressions;

namespace Chirp.Razor.Repository
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        Task<T> Get(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAll();   
    }
}

