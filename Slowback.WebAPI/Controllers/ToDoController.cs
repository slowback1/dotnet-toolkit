using Microsoft.AspNetCore.Mvc;
using Slowback.Common;

namespace Slowback.WebAPI.Controllers;

[Route("todos")]
public class ToDoController : BaseController
{
    [HttpGet]
    [Route("")]
    public string Get()
    {
        MessageBus.MessageBus.GetInstance().Publish(Messages.LogMessage, "Hello World!");

        return "Hello World!";
    }
}