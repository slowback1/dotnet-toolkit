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
}