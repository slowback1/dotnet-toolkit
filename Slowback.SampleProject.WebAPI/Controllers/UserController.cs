using Microsoft.AspNetCore.Mvc;
using Slowback.Common;
using Slowback.SampleProject.Common.Dtos;
using Slowback.SampleProject.Data.UnitOfWork;
using Slowback.SampleProject.Data.User;
using Slowback.SampleProject.Logic.Authentication;

namespace Slowback.SampleProject.WebAPI.Controllers;

[Route("users")]
public class UserController : BaseController
{
    public UserController()
    {
    }

    public UserController(UnitOfWorkType type) : base(type)
    {
    }

    [HttpPost]
    [Route("create")]
    public async Task<ApiResponse<GetUser>> Create([FromBody] CreateUser createUser)
    {
        var creator = new UserCreator(_context);

        var user = await creator.CreateUser(createUser);

        var stored = await new UserRetriever(_context).GetUserById(user);

        return stored is not null ? Wrap(stored!) : Wrap<GetUser>(null);
    }

    [HttpPost]
    [Route("login")]
    public async Task<ApiResponse<string>> Login([FromBody] LoginDto loginUser)
    {
        var stored = await new UserRetriever(_context).GetUserById(loginUser.Id);

        if (stored is null) return NoContent<string>();

        var handler = new UserAuthenticator();

        var token = handler.CreateToken(stored.Id);

        return Wrap(token);
    }
}