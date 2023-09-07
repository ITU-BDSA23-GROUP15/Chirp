namespace SimpleDB;

public interface IDatabaseRepository
{
    public IEnumerable<T> Read(int? limit = null);
    public void Store(T record);
}