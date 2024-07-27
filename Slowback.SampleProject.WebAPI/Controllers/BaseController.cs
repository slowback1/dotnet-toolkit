using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Slowback.Common;
using Slowback.Data.Core.EF;
using Slowback.SampleProject.Data.Core;

namespace Slowback.SampleProject.WebAPI.Controllers;

public class BaseController : Controller
{
    protected readonly SampleAppContext _context;

    public BaseController(IConfiguration config)
    {
        _context = LoadAppContext(config);
    }

    private SampleAppContext LoadAppContext(IConfiguration config)
    {
        var databaseConfig = config.GetSection("Database").Get<ConnectionOptions>()!;

        return new SampleAppContext(databaseConfig);
    }


    public override void OnActionExecuting(ActionExecutingContext context)
    {
        ValidateInputs(context);
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
}