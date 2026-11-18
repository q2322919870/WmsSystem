using Microsoft.AspNetCore.Mvc;
using Wms.Api.Common;
using Wms.Application.DTOs;
using Wms.Application.Services;

namespace Wms.Api.Controllers;
[ApiController]
[Route("api/[controller]/[action]")]
public class OutboundOrdersController : Controller
{
    private readonly OutboundOrderService _service;
    public OutboundOrdersController(OutboundOrderService service)=> _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(ApiResponse.Success(await _service.GetAllAsync(),"查询成功"));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOutboundOrderDto dto)
    {
        await _service.CreateOutboundOrderAsync(dto);
        return Ok(ApiResponse.Success("创建成功"));
    }

    [HttpPost("{id}/allocate")]
    public async Task<IActionResult> Allocate(int id, [FromBody] AllocateOutboundDto dto)
    {
        await _service.AllocateAsync(id, dto);
        return Ok(ApiResponse.Success("分配成功（库存已锁定）"));
    }
    [HttpPost("{id}/pick")]
    public async Task<IActionResult> Pick(int id, [FromBody] PickOutboundDto dto)
    {
        await _service.PickAsync(id, dto);
        return Ok(ApiResponse.Success("拣货成功"));
    }
    [HttpPost("{id}/check")]
    public async Task<IActionResult> Pick(int id, [FromBody] CheckOutboundDto dto)
    {
        await _service.CheckAsync(id, dto);
        return Ok(ApiResponse.Success("复核成功"));
    }
    [HttpPost("{id}/ship")]
    public async Task<IActionResult> Ship(int id,[FromBody] ShipOutboundDto dto)
    {
        await _service.ShipAsync(id,dto.DetailId,dto.OperatorName);
        return Ok(ApiResponse.Success("出库成功"));
    }
}