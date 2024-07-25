namespace SampleDatabase.Storage;

public class InMemoryStorage<T>
{
    private readonly List<T> _items = new();

    public Task AddAsync(T item)
    {
        _items.Add(item);
        return Task.CompletedTask;
    }

    public Task<List<T>> GetAllAsync()
    {
        return Task.FromResult(_items);
    }

    public Task<T?> GetAsync(Func<T, bool> predicate)
    {
        return Task.FromResult(_items.FirstOrDefault(predicate));
    }

    public Task UpdateAsync(Predicate<T> predicate, T item)
    {
        var index = _items.FindIndex(predicate);
        if (index != -1) _items[index] = item;
        return Task.CompletedTask;
    }

    public Task<int> CountAsync(Func<T, bool> predicate)
    {
        return Task.FromResult(_items.Count(predicate));
    }

    public async Task<int> CountAsync()
    {
        return await CountAsync(t => true);
    }

    public async Task ClearAsync()
    {
        _items.Clear();
        await Task.CompletedTask;
    }
}