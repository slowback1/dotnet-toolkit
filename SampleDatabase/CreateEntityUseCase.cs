using SampleDatabase.Models;
using SampleDatabase.Storage;
using SampleDataModels;
using Slowback.MessageBus;

namespace SampleDatabase;

public class CreateEntityUseCase
{
    public static readonly string CreateEntityMessage = "CREATE_ENTITY";
    public static readonly string CreateEntitySuccessMessage = "CREATE_ENTITY_SUCCESS";

    static CreateEntityUseCase()
    {
        MessageBus.GetInstance().Subscribe<SampleDto?>(CreateEntityMessage, async message =>
        {
            if (message == null) return;

            var useCase = new CreateEntityUseCase();
            await useCase.ExecuteAsync(message);
        });
    }

    public async Task ExecuteAsync(SampleDto entity)
    {
        await DataStore.SampleEntities.AddAsync(new SampleEntity
        {
            Description = entity.Description,
            Name = entity.Name,
            IsActive = entity.IsActive,
            Id = await DataStore.SampleEntities.CountAsync() + 1
        });

        await MessageBus.GetInstance().PublishAsync(CreateEntitySuccessMessage, entity);
    }
}