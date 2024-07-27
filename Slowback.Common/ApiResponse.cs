using System.Collections.Generic;

namespace Slowback.Common;

public class ApiResponse<T>
{
    public T? Data { get; set; }
    public List<string> ErrorMessages { get; set; } = new();
    public string Status => ErrorMessages.Count > 0 ? ApiResponseStatus.Error : ApiResponseStatus.Ok;
}

public static class ApiResponseStatus
{
    public const string Ok = "Ok";
    public const string Error = "Error";
}