using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Slowback.Common;

namespace Slowback.WebAPI.Controllers;

public class BaseController : Controller
{
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
}