using SampleDatabase.Models;

namespace SampleDatabase.Storage;

public static class DataStore
{
    public static InMemoryStorage<SampleEntity> SampleEntities { get; } = new();
}