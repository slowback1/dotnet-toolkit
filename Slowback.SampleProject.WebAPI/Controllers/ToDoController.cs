using Microsoft.AspNetCore.Mvc;
using Slowback.Common;
using Slowback.SampleProject.Common.Dtos;
using Slowback.SampleProject.Data.ToDo;

namespace Slowback.SampleProject.WebAPI.Controllers;

[Route("todos")]
public class ToDoController : BaseController
{
    public ToDoController(IConfiguration config) : base(config)
    {
    }

    [HttpGet]
    [Route("")]
    public async Task<ApiResponse<List<GetToDo>>> Get()
    {
        var retriever = new ToDoRetriever(_context);

        var todos = await retriever.GetToDos();

        return Wrap(todos.ToList());
    }

    [HttpPost]
    [Route("")]
    public async Task<ApiResponse<GetToDo>> Create([FromBody] CreateToDo createToDo)
    {
        var creator = new ToDoCreator(_context);

        var todo = await creator.CreateToDo(createToDo);

        var retriever = new ToDoRetriever(_context);

        var createdToDo = await retriever.GetToDoById(todo);

        return Wrap(createdToDo);
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<ApiResponse<GetToDo>> Edit(int id, [FromBody] EditToDo editToDo)
    {
        var editor = new ToDoUpdater(_context);

        editToDo.Id = id;

        var todo = await editor.UpdateToDo(editToDo);

        var retriever = new ToDoRetriever(_context);

        var editedToDo = await retriever.GetToDoById(todo);

        return Wrap(editedToDo);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ApiResponse<GetToDo>> Get(int id)
    {
        var retriever = new ToDoRetriever(_context);

        var todo = await retriever.GetToDoById(id);

        return Wrap(todo);
    }
}