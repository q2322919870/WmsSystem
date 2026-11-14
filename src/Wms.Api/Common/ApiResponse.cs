namespace Wms.Api.Common;

public class ApiResponse
{
    public int Code { get; set; }
    public string Message { get; set; } = string.Empty;
    public object? Data { get; set; }

    public static ApiResponse Success(string message = "操作成功")
    {
        return new ApiResponse { Code = 200, Message = message };
    }
    public static ApiResponse<T> Success<T>(T data, string message = "操作成功")
    {
        return new ApiResponse<T> { Code = 200, Message = message, Data = data };
    }
    public static ApiResponse Fail(string message = "操作失败", int code = 400)
    {
        return new ApiResponse { Code = code, Message = message };
    }
}
public class ApiResponse<T> : ApiResponse
{
    public new T? Data { get; set; }
}