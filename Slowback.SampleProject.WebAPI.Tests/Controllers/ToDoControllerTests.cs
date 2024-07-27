using Microsoft.AspNetCore.Mvc;
using Slowback.SampleProject.WebAPI.Controllers;
using Slowback.TestUtilities;

namespace Slowback.SampleProject.WebAPI.Tests.Controllers;

public class ToDoControllerTests
{
    [Test]
    public void ToDoControllerHasRouteAttribute()
    {
        typeof(ToDoController)
            .HasAttribute<RouteAttribute>();
    }
}