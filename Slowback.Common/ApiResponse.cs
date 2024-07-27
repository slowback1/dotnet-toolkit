using System.Collections.Generic;

namespace Slowback.Common;

public class ApiResponse<T>
{
    public T? Data { get; set; }
    public List<string> ErrorMessages { get; set; } = new();
}