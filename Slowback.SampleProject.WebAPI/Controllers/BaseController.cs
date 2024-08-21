using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Slowback.Common;
using Slowback.Data.Core.EF;
using Slowback.Messaging;
using Slowback.SampleProject.Data.Core;
using Slowback.SampleProject.Logic.Authentication;

namespace Slowback.SampleProject.WebAPI.Controllers;

public class BaseController : Controller
{
    protected readonly SampleAppContext _context;

    public BaseController(IConfiguration config)
    {
        _context = LoadAppContext(config);
    }

    protected string? UserId { get; set; }

    private SampleAppContext LoadAppContext(IConfiguration config)
    {
        var databaseConfig = MessageBus.GetLastMessage<ConnectionOptions>(Messages.AppDbConnection);

        return new SampleAppContext(databaseConfig);
    }


    public override void OnActionExecuting(ActionExecutingContext context)
    {
        GetUserId();
        ValidateInputs(context);
    }

    private void GetUserId()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        var handler = new UserAuthenticator();

        UserId = handler.ValidateToken(token);
    }

    private void ValidateInputs(ActionExecutingContext context)
    {
        var values = context.ActionArguments.Values;

        var validationErrors = new List<string>();

        foreach (var value in values) validationErrors.AddRange(Validator.ObjectValidator.ValidateObject(value));

        if (validationErrors.Count > 0)
            context.Result = Ok(new ApiResponse<object> { ErrorMessages = validationErrors });
    }

    protected ApiResponse<T> Wrap<T>(T data)
    {
        return new ApiResponse<T> { Data = data };
    }

    protected ApiResponse<T> NoContent<T>()
    {
        return new ApiResponse<T> { Data = default, ErrorMessages = new List<string> { "No content found." } };
    }
}