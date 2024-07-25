using Microsoft.AspNetCore.Mvc;
using SampleDatabase;
using SampleDatabase.Storage;
using SampleDataModels;
using Slowback.MessageBus;

namespace SampleApi.Controllers;

[Route("Sample")]
public class SampleController : Controller
{
    // GET
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index()
    {
        var entities = await DataStore.SampleEntities.GetAllAsync();

        return Ok(entities);
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> Create([FromBody] SampleDto dto)
    {
        await MessageBus.GetInstance().PublishAsync(CreateEntityUseCase.CreateEntityMessage, dto);

        return Ok(dto);
    }
}