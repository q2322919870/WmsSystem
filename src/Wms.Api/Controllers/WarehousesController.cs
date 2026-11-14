using Microsoft.AspNetCore.Mvc;
using Wms.Api.Common;
using Wms.Application.DTOs;
using Wms.Application.Services;

namespace Wms.Api.Controllers;
[ApiController]
[Route("api/[controller]/[action]")]
public class WarehousesController : Controller
{
    private readonly WarehouseService _service;

    public WarehousesController(WarehouseService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllWarehouses();
        return Ok(ApiResponse.Success(result,"获取成功"));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWarehouseDto dto)
    {
        await _service.CreateWarehouse(dto);
        return Ok(ApiResponse.Success("创建成功"));
    }
    
}