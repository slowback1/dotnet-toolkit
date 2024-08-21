using Microsoft.EntityFrameworkCore;
using Slowback.Mapper;
using Slowback.SampleProject.Common.Dtos;
using Slowback.SampleProject.Data.Core;

namespace Slowback.SampleProject.Data.User;

public class UserEditor : BaseDatabaseAction
{
    public UserEditor(SampleAppContext context) : base(context)
    {
    }

    public async Task UpdateUser(EditUser dto)
    {
        var user = await GetUserById(dto.Id);

        if (user == null) return;

        dto.ConvertToModel().Map(user);

        await _context.SaveChangesAsync();
    }

    private async Task<Core.Models.User?> GetUserById(string userId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
    }
}