using Microsoft.AspNetCore.Mvc;
using Wms.Api.Common;
using Wms.Application.DTOs;
using Wms.Application.Services;

namespace Wms.Api.Controllers;
[ApiController]
[Route("api/[controller]/[action]")]
public class StocktakeOrdersController : Controller
{
    private readonly StocktakeOrderService _service;

    public StocktakeOrdersController(StocktakeOrderService service)
    => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var results = await _service.GetAllAsync();
        return Ok(ApiResponse.Success(results,"查询成功"));
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStocktakeOrderDto dto)
    {
        await _service.CreateAsync(dto);
        return Ok(ApiResponse.Success("盘点单创建成功"));
    }

    [HttpPost("{id}/submit")]
    public async Task<IActionResult> Submit(int id, [FromBody] SubmitStocktakeOrderDto dto)
    {
        await _service.SubmitAsync(id, dto.ReviewerName);
        return Ok(ApiResponse.Success("盘点提交成功，库存已调整"));
    }
}