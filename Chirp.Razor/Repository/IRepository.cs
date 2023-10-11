public interface IRepository<T> where T : class {
    T Add(T entity);
    T Get(int id);
    IEnumerable<T> GetAll();   
}

