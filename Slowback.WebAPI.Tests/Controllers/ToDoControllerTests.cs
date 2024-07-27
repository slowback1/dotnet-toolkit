using Microsoft.AspNetCore.Mvc;
using Slowback.TestUtilities;
using Slowback.WebAPI.Controllers;

namespace Slowback.WebAPI.Tests.Controllers;

public class ToDoControllerTests
{
    [Test]
    public void ToDoControllerHasRouteAttribute()
    {
        typeof(ToDoController)
            .HasAttribute<RouteAttribute>();
    }
}