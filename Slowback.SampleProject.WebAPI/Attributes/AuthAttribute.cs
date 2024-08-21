using Microsoft.AspNetCore.Mvc.Filters;
using Slowback.SampleProject.Logic.Authentication;

namespace Slowback.SampleProject.WebAPI.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class AuthAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var hasAuthHeader = context.HttpContext.Request.Headers.TryGetValue("Authorization", out var token);

        if (!hasAuthHeader)
        {
            Set401Response(context);
            return;
        }

        var tokenString = token.ToString().Replace("Bearer ", "");

        var handler = new UserAuthenticator();

        var userId = handler.ValidateToken(tokenString);

        var isValid = userId != null;

        if (!isValid) Set401Response(context);
    }

    private void Set401Response(AuthorizationFilterContext context)
    {
        context.HttpContext.Response.StatusCode = 401;
        context.HttpContext.Response.Headers.Add("WWW-Authenticate", "Bearer");
    }
}