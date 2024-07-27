using Microsoft.EntityFrameworkCore;
using Slowback.Data.Core.EF;
using Slowback.SampleProject.Data.Core.Models;

namespace Slowback.SampleProject.Data.Core;

public class SampleAppContext : BaseContext
{
    public SampleAppContext(ConnectionOptions connectionOptions) : base(connectionOptions)
    {
    }

    public virtual DbSet<ToDo> ToDos { get; set; }
}