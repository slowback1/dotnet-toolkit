using Microsoft.AspNetCore.Mvc;
using Slowback.Common;
using Slowback.Messaging;

namespace Slowback.WebAPI.Controllers;

[Route("todos")]
public class ToDoController : BaseController
{
    [HttpGet]
    [Route("")]
    public string Get()
    {
        MessageBus.Publish(Messages.LogMessage, "Hello World!");

        return "Hello World!";
    }
}