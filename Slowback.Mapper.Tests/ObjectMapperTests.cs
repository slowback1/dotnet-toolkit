namespace Slowback.Mapper.Tests;

public class ObjectMapperTests
{
    [Test]
    public void Map_ShouldCopyProperties()
    {
        var source = new TestClass { Id = 1, Name = "Source" };
        var destination = new TestClass();

        source.Map(destination);

        Assert.That(destination.Id, Is.EqualTo(source.Id));
        Assert.That(destination.Name, Is.EqualTo(source.Name));
    }

    [Test]
    public void Map_ShouldThrowArgumentNullException_WhenSourceIsNull()
    {
        TestClass source = null;
        var destination = new TestClass();

        Assert.Throws<ArgumentNullException>(() => source.Map(destination));
    }

    [Test]
    public void Map_ShouldThrowArgumentNullException_WhenDestinationIsNull()
    {
        var source = new TestClass();
        TestClass destination = null;

        Assert.Throws<ArgumentNullException>(() => source.Map(destination));
    }

    [Test]
    public void Map_ShouldIgnoreNestedClass()
    {
        var source = new ParentClass
        {
            Id = 1,
            Name = "Source",
            Nested = new NestedClass { NestedId = 10 }
        };
        var destination = new ParentClass();

        source.Map(destination);

        Assert.That(destination.Id, Is.EqualTo(source.Id));
        Assert.That(destination.Name, Is.EqualTo(source.Name));
        Assert.That(destination.Nested, Is.Null);
    }

    [Test]
    public void Map_ShouldIgnoreIEnumerable()
    {
        var source = new CollectionClass
        {
            Id = 1,
            Name = "Source",
            Numbers = new List<int> { 1, 2, 3 }
        };
        var destination = new CollectionClass();

        source.Map(destination);

        Assert.That(destination.Id, Is.EqualTo(source.Id));
        Assert.That(destination.Name, Is.EqualTo(source.Name));
        Assert.That(destination.Numbers, Is.Null);
    }

    [Test]
    public void Map_ShouldIgnorePropertiesWithMapperIgnoreAttribute()
    {
        var source = new IgnoredPropertyClass
        {
            Id = 1,
            Name = "Source",
            IgnoredProperty = "Should be ignored"
        };
        var destination = new IgnoredPropertyClass();

        source.Map(destination);

        Assert.That(destination.Id, Is.EqualTo(source.Id));
        Assert.That(destination.Name, Is.EqualTo(source.Name));
        Assert.That(destination.IgnoredProperty, Is.Null);
    }

    private class IgnoredPropertyClass
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [MapperIgnore]
        public string IgnoredProperty { get; set; }
    }

    private class TestClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    private class NestedClass
    {
        public int NestedId { get; set; }
    }

    private class ParentClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public NestedClass Nested { get; set; }
    }

    private class CollectionClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<int> Numbers { get; set; }
    }
}