namespace SimpleDB;

public interface IDatabaseRepository<T>
{

    public IEnumerable<T> Read();
    public void Store(T record);
}