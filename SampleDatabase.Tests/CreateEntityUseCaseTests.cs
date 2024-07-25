using SampleDatabase.Storage;
using SampleDataModels;
using Slowback.MessageBus;

namespace SampleDatabase.Tests;

public class CreateEntityUseCaseTests
{
    [TearDown]
    public async Task TearDown()
    {
        await DataStore.SampleEntities.ClearAsync();
        MessageBus.GetInstance().ClearSubscribers();
        MessageBus.GetInstance().ClearMessages();
    }

    [Test]
    public async Task ExecuteCreatesAnEntity()
    {
        var useCase = new CreateEntityUseCase();
        var entity = new SampleDto { Name = "Test", Description = "Description", IsActive = true };

        await useCase.ExecuteAsync(entity);

        var entities = await DataStore.SampleEntities.GetAllAsync();

        Assert.That(entities.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task ExecuteCreatesAnEntityWithCorrectValues()
    {
        var useCase = new CreateEntityUseCase();
        var entity = new SampleDto { Name = "Test", Description = "Description", IsActive = true };

        await useCase.ExecuteAsync(entity);

        var entities = await DataStore.SampleEntities.GetAllAsync();

        Assert.That(entities.Count, Is.EqualTo(1));
        Assert.That(entities[0].Name, Is.EqualTo("Test"));
        Assert.That(entities[0].Description, Is.EqualTo("Description"));
        Assert.That(entities[0].IsActive, Is.True);
    }

    [Test]
    public async Task ExecuteCreatesMultipleEntities()
    {
        var useCase = new CreateEntityUseCase();
        var entity1 = new SampleDto { Name = "Test1", Description = "Description1", IsActive = true };
        var entity2 = new SampleDto { Name = "Test2", Description = "Description2", IsActive = false };

        await useCase.ExecuteAsync(entity1);
        await useCase.ExecuteAsync(entity2);

        var entities = await DataStore.SampleEntities.GetAllAsync();

        Assert.That(entities.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task ExecuteCreatesAnIdForTheEntity()
    {
        var useCase = new CreateEntityUseCase();
        var entity = new SampleDto { Name = "Test", Description = "Description", IsActive = true };

        await useCase.ExecuteAsync(entity);

        var entities = await DataStore.SampleEntities.GetAllAsync();

        Assert.That(entities.Count, Is.EqualTo(1));
        Assert.That(entities[0].Id, Is.EqualTo(1));
    }

    [Test]
    public async Task IncrementsTheIdWhenCreatingAnEntity()
    {
        var useCase = new CreateEntityUseCase();
        var entity1 = new SampleDto { Name = "Test1", Description = "Description1", IsActive = true };
        var entity2 = new SampleDto { Name = "Test2", Description = "Description2", IsActive = false };

        await useCase.ExecuteAsync(entity1);
        await useCase.ExecuteAsync(entity2);

        var entities = await DataStore.SampleEntities.GetAllAsync();

        Assert.That(entities.Count, Is.EqualTo(2));
        Assert.That(entities[0].Id, Is.EqualTo(1));
        Assert.That(entities[1].Id, Is.EqualTo(2));
    }

    [Test]
    public async Task CanCreateAnEntityViaAMessageBusMessage()
    {
        var bus = MessageBus.GetInstance();

        await bus.PublishAsync(CreateEntityUseCase.CreateEntityMessage, new SampleDto
        {
            Description = "Description",
            IsActive = true,
            Name = "Test"
        });

        var entities = await DataStore.SampleEntities.GetAllAsync();

        Assert.That(entities.Count, Is.EqualTo(1));
    }
}