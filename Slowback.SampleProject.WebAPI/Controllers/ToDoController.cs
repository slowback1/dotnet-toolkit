using Microsoft.AspNetCore.Mvc;
using Slowback.Common;
using Slowback.SampleProject.Common.Dtos;
using Slowback.SampleProject.Data.UnitOfWork;
using Slowback.SampleProject.WebAPI.Attributes;

namespace Slowback.SampleProject.WebAPI.Controllers;

[Route("todos")]
[Auth]
public class ToDoController : BaseController
{
    public ToDoController()
    {
    }

    public ToDoController(UnitOfWorkType type) : base(type)
    {
    }

    [HttpGet]
    [Route("")]
    public async Task<ApiResponse<List<GetToDo>>> Get()
    {
        var retriever = _unitOfWork.ToDoRetriever;

        var todos = await retriever.GetToDosForUser(UserId!);

        return Wrap(todos.ToList());
    }

    [HttpPost]
    [Route("")]
    public async Task<ApiResponse<GetToDo>> Create([FromBody] CreateToDo createToDo)
    {
        var creator = _unitOfWork.ToDoCreator;

        var todo = await creator.CreateToDo(createToDo, UserId!);

        var retriever = _unitOfWork.ToDoRetriever;

        var createdToDo = await retriever.GetToDoById(todo);

        return Wrap(createdToDo);
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<ApiResponse<GetToDo>> Edit(int id, [FromBody] EditToDo editToDo)
    {
        var editor = _unitOfWork.ToDoUpdater;

        editToDo.Id = id;

        var todo = await editor.UpdateToDo(editToDo);

        var retriever = _unitOfWork.ToDoRetriever;

        var editedToDo = await retriever.GetToDoById(todo);

        return Wrap(editedToDo);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ApiResponse<GetToDo>> Get(int id)
    {
        var retriever = _unitOfWork.ToDoRetriever;

        var todo = await retriever.GetToDoById(id);

        return Wrap(todo);
    }
}