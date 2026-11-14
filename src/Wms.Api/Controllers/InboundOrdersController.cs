using Microsoft.AspNetCore.Mvc;
using Wms.Api.Common;
using Wms.Application.DTOs;
using Wms.Application.Services;

namespace Wms.Api.Controllers;
[ApiController]
[Route("api/[controller]/[action]")]
public class InboundOrdersController : Controller
{
    private readonly InboundOrderService _service;

    public InboundOrdersController(InboundOrderService service)
    {
        _service = service;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(ApiResponse.Success(result,"查询成功"));
    }
    [HttpGet]
    public async Task<IActionResult> GetByIdForReceiving(int id)
    {
        var result = await _service.GetByIdForReceivingAsync(id);
        return Ok(ApiResponse.Success(result,"查询成功"));
    }
    [HttpGet]
    public async Task<IActionResult> GetBasicById(int id)
    {
        var result = await _service.GetBasicByIdAsync(id);
        return Ok(ApiResponse.Success(result,"查询成功"));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateInboundOrderDto dto)
    {
        await _service.CreateAsync(dto);
        return Ok(ApiResponse.Success("创建成功"));
    }

    [HttpPost("{id}/receive")]
    public async Task<IActionResult> Receive(int id, [FromBody] ReceiveInboundDto dto)
    {
        await _service.ReceiveAsync(id, dto);//id是入库单id，dto是收货信息
        return Ok(ApiResponse.Success("收货成功"));
    }
}