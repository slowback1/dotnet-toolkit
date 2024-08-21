using Slowback.SampleProject.Common.Dtos;

namespace Slowback.SampleProject.Data.User;

internal static class UserConverters
{
    internal static GetUser ConvertToDto(this Core.Models.User user)
    {
        return new GetUser
        {
            Id = user.Id.ToString(),
            Name = user.Name
        };
    }

    internal static Core.Models.User ConvertToModel(this CreateUser dto, Guid id)
    {
        return new Core.Models.User
        {
            Id = id,
            Name = dto.Name
        };
    }

    internal static Core.Models.User ConvertToModel(this EditUser dto)
    {
        return new Core.Models.User
        {
            Id = new Guid(dto.Id),
            Name = dto.Name
        };
    }
}