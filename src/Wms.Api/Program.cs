using Microsoft.EntityFrameworkCore;
using Wms.Api.Middleware;
using Wms.Application.Services;
using Wms.Domain.Interfaces;
using Wms.Infrastructure.Persistence;
using Wms.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<WmsDbContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();
builder.Services.AddScoped<IStorageAreaRepository, StorageAreaRepository>();
builder.Services.AddScoped<IStorageLocationRepository, StorageLocationRepository>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
builder.Services.AddScoped<IInboundOrderRepository, InboundOrderRepository>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IOutboundOrderRepository, OutboundOrderRepository>();
builder.Services.AddScoped<OutboundOrderService>();
builder.Services.AddScoped<InventoryService>();
builder.Services.AddScoped<InboundOrderService>();
builder.Services.AddScoped<MaterialService>();
builder.Services.AddScoped<StorageLocationService>();
builder.Services.AddScoped<StorageAreaService>();
builder.Services.AddScoped<WarehouseService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();//EndpointsApiExplorer用于生成OpenAPI文档，它会扫描应用程序中的所有端点，并生成相应的OpenAPI规范。
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
// 注册全局异常处理中间件（必须放在 UseAuthorization 之前）
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthorization();
app.MapControllers();//MapControllers()方法用于将控制器映射到路由。它会扫描应用程序中的所有控制器，并根据控制器上的路由属性生成相应的路由映射。
app.UseHttpsRedirection();//UseHttpsRedirection()方法用于将HTTP请求重定向到HTTPS。它会检查请求的协议，如果是HTTP，则会返回一个重定向响应，指示客户端使用HTTPS进行请求。

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}