using System.Net;
using System.Text.Json;
using Wms.Api.Common;
using Wms.Domain.Exceptions;

namespace Wms.Api.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);

        }
        catch (BusinessException ex)
        {
            _logger.LogWarning(ex, $"业务异常: {ex.Message}");
            await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message, ex.Code);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"系统异常: {ex.Message}");
            await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "系统内部错误，请联系管理员", 500);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message, int code = 400)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        var response = ApiResponse.Fail(message, code);
        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions()//JsonSerializer.Serialize方法用于将对象序列化为JSON字符串。它接受两个参数：要序列化的对象和一个JsonSerializerOptions对象，用于配置序列化行为。
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase//PropertyNamingPolicy属性用于指定序列化时的属性命名策略。JsonNamingPolicy.CamelCase表示将属性名转换为camelCase格式。
        });
        await context.Response.WriteAsync(json);
    }
}